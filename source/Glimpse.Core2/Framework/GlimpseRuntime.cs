using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;

namespace Glimpse.Core2.Framework
{
    public class GlimpseRuntime
    {
        public GlimpseRuntime(GlimpseConfiguration configuration)
        {
            //Version is in major.minor.build format to support http://semver.org/
            Version = GetType().Assembly.GetName().Version.ToString(3);
            UpdateConfiguration(configuration);
        }


        private GlimpseConfiguration Configuration { get; set; }
        //Todo: wrap in an IDataStore
        private IDictionary<string, object> PluginResultsStore
        {
            get
            {
                var requestStore = Configuration.FrameworkProvider.HttpRequestStore;
                var result = requestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);

                if (result == null)
                {
                    result = new Dictionary<string, object>();
                    requestStore.Set(Constants.PluginResultsDataStoreKey, result);
                }

                return result;
            }
        }

        public IServiceLocator ServiceLocator
        {
            get
            {
                var result =
                    Configuration.FrameworkProvider.HttpRequestStore.Get<GlimpseServiceLocator>(
                        Constants.ServiceLocatorKey);

                if (result == null)
                    throw new MethodAccessException(Resources.OutOfOrderRuntimeMethodCall);

                return result;
            }
        }

        public string Version { get; private set; }


        //TODO: Make sure runtime has been init'ed
        public void BeginRequest()
        {
            var mode = GetGlimpseMode(RuntimePhase.BeginRequest);
            if (mode == GlimpseMode.Off) return;

            var frameworkProvider = Configuration.FrameworkProvider;
            var runtimeContext = frameworkProvider.RuntimeContext;
            var requestStore = frameworkProvider.HttpRequestStore;

            //Create storage space for plugins to access
            var pluginStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
            requestStore.Set(Constants.PluginsDataStoreKey, pluginStore);

            //Create ServiceLocator valid for this request
            requestStore.Set(Constants.ServiceLocatorKey, new GlimpseServiceLocator(runtimeContext, pluginStore, Configuration.PipelineInspectors));

            //Give Request an ID
            requestStore.Set(Constants.RequestIdKey, Guid.NewGuid());

            //Create and start global stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            requestStore.Set(Constants.GlobalStopwatchKey, stopwatch);
        }

        //Todo: Make sure request has begun?
        public void EndRequest()
        {
            var mode = GetGlimpseMode(RuntimePhase.EndRequest);
            if (mode == GlimpseMode.Off) return;

            var encoder = Configuration.HtmlEncoder;
            var frameworkProvider = Configuration.FrameworkProvider;
            var serializer = Configuration.Serializer;
            //TODO: Store data and name
            var pluginResults = PluginResultsStore.ToDictionary(item => item.Key, item => serializer.Serialize(item.Value));
            var requestMetadata = frameworkProvider.RequestMetadata;
            var requestStore = frameworkProvider.HttpRequestStore;
            var resourceEndpoint = Configuration.ResourceEndpoint;
            Guid requestId;

            try
            {
                requestId = requestStore.Get<Guid>(Constants.RequestIdKey);
            }
            catch (NullReferenceException ex)
            {
                throw new MethodAccessException(Resources.OutOfOrderRuntimeMethodCall, ex);
            }

            var metadata = new GlimpseMetadata(requestId, requestMetadata, pluginResults);

            //TODO: Handle exceptions
            Configuration.PersistanceStore.Save(metadata);

            //TODO: Filter out requests that should not have the ID header
            frameworkProvider.SetHttpResponseHeader(Constants.HttpHeader, requestId.ToString());

            var dataPath = encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUrl("data.js", Version, new Dictionary<string, string>{{"id", requestId.ToString()}}));
            var clientPath = encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUrl("client.js", Version));

            //var dataPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId());
            //var clientPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("client.js"));

            var html = string.Format(@"<script type='text/javascript' id='glimpseData' src='{0}'></script><script type='text/javascript' id='glimpseClient' src='{1}'></script></body>", dataPath, clientPath);

            frameworkProvider.InjectHttpResponseBody(html);
        }

        public void ExecuteResource(string resourceName)
        {
            ExecuteResource(resourceName, new Dictionary<string, string>());
        }

        public void ExecuteResource(string resourceName, IDictionary<string, string> parameters)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(resourceName), "resourceName");
            
            var mode = GetGlimpseMode(RuntimePhase.ExecuteResource);
            if (mode == GlimpseMode.Off) return;

            var logger = Configuration.Logger;
            ResourceResult result;

            var resources = Configuration.Resources.Where(r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

            switch (resources.Count())
            {
                case 1: //200 - OK
                    try
                    {
                        var param = parameters ?? new Dictionary<string, string>();
                        result = resources.First().Execute(param);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("Error executing resource '{0}'", resourceName), ex);
                        result = new ExceptionResourceResult(ex);
                    }
                    break;
                case 0: //404 - File Not Found
                    logger.Warn(string.Format(Resources.ExecuteResourceMissingError, resourceName));
                    result = new StatusCodeResourceResult(404);
                    break;
                default: //500 - Server Error
                    logger.Warn(string.Format(Resources.ExecuteResourceDuplicateError, resourceName));
                    result = new StatusCodeResourceResult(500);
                    break;
            }

            try
            {
                result.Execute(Configuration.FrameworkProvider);
            }
            catch(Exception ex)
            {
                logger.Fatal(string.Format("Error executing resource result of type '{0}'", result.GetType()), ex);
            }
        }

        public void ExecuteTabs()
        {
            ExecuteTabs(LifeCycleSupport.EndRequest);
        }

        public void ExecuteTabs(LifeCycleSupport support)
        {
            var mode = GetGlimpseMode(RuntimePhase.ExecuteTabs);
            if (mode == GlimpseMode.Off) return;

            //Only use tabs that either don't specify a specific context type, or have a context type that matches the current framework provider's.
            var runtimePlugins =
                Configuration.Tabs.Where(
                    p =>
                    p.Metadata.RequestContextType == null ||
                    p.Metadata.RequestContextType == Configuration.FrameworkProvider.RuntimeContextType);

            var supportedRuntimePlugins = runtimePlugins.Where(p => p.Metadata.LifeCycleSupport.HasFlag(support));
            var pluginResultsStore = PluginResultsStore;
            var logger = Configuration.Logger;

            foreach (var plugin in supportedRuntimePlugins)
            {
                var key = plugin.Value.GetType().FullName;
                try
                {
                    if (pluginResultsStore.ContainsKey(key))
                        pluginResultsStore[key] = plugin.Value.GetData(ServiceLocator);
                    else
                        pluginResultsStore.Add(key, plugin.Value.GetData(ServiceLocator));
                }
                catch (Exception exception)
                {
                    logger.Error(string.Format(Resources.ExecuteTabError, key), exception);
                }
            }
        }

        //Todo: Set an "Init'ed" bit
        public GlimpseMode Initialize()
        {
            var logger = Configuration.Logger;
            var mode = GetGlimpseMode(RuntimePhase.Initialize);
            if (mode == GlimpseMode.Off) return GlimpseMode.Off;

            //TODO: Add in request validation checks
            var tabsThatRequireSetup = Configuration.Tabs.Where(p => p.Value is IGlimpseTabSetup).Select(p => p.Value);
            foreach (IGlimpseTabSetup tab in tabsThatRequireSetup)
            {
                try
                {
                    tab.Setup();
                }
                catch (Exception exception)
                {
                    logger.Error(string.Format(Resources.InitializeTabError, tab.GetType().FullName), exception);
                }
            }

            foreach (var pipelineInspector in Configuration.PipelineInspectors)
            {
                try
                {
                    pipelineInspector.Setup();
                }
                catch (Exception exception)
                {
                    logger.Error(
                        string.Format(Resources.InitializePipelineInspectorError, pipelineInspector.GetType().FullName),
                        exception);
                }
            }

            return mode;
        }

        //TODO: Test that these collections are auto populated
        public void UpdateConfiguration(GlimpseConfiguration configuration)
        {
            //TODO: destruct modifiers and plugins
            if (configuration.Tabs.Discoverability.AutoDiscover)
                configuration.Tabs.Discoverability.Discover();

            if (configuration.PipelineInspectors.Discoverability.AutoDiscover)
                configuration.PipelineInspectors.Discoverability.Discover();

            if (configuration.Resources.Discoverability.AutoDiscover)
                configuration.Resources.Discoverability.Discover();

            if (configuration.Validators.Discoverability.AutoDiscover)
                configuration.Validators.Discoverability.Discover();

            Configuration = configuration;
        }

        private GlimpseMode GetGlimpseMode(RuntimePhase runtimePhase)
        {
            var requestStore = Configuration.FrameworkProvider.HttpRequestStore;
            var result = requestStore.Contains(Constants.GlimpseModeKey) ? requestStore.Get<GlimpseMode>(Constants.GlimpseModeKey) : Configuration.Mode;

            if (result != GlimpseMode.Off)
            {
                var validators = Configuration.Validators.Where(v => !v.Metadata.RuntimePhase.HasValue || v.Metadata.RuntimePhase.Value.HasFlag(runtimePhase));

                var requestMetadata = Configuration.FrameworkProvider.RequestMetadata;
                foreach (var validator in validators)
                {
                    //TODO: Handle exceptions from validator
                    var mode = validator.Value.GetMode(requestMetadata);

                    //Only use the lowest level allowed for the request
                    if (mode < result) result = mode;
                }
            }

            requestStore.Set(Constants.GlimpseModeKey, result);
            return result;
        }
    }
}