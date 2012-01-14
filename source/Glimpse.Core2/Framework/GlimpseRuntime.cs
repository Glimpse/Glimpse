using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;

namespace Glimpse.Core2.Framework
{
    public class GlimpseRuntime : IGlimpseRuntime
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

        public bool IsInitialized { get; private set; }

        public string Version { get; private set; }


        //TODO: Make sure runtime has been init'ed
        public void BeginRequest()
        {
            var mode = GetRuntimePolicy(RuntimeEvent.BeginRequest);
            if (mode == RuntimePolicy.Off) return;

            var frameworkProvider = Configuration.FrameworkProvider;
            var runtimeContext = frameworkProvider.RuntimeContext;
            var requestStore = frameworkProvider.HttpRequestStore;

            //Give Request an ID
            requestStore.Set(Constants.RequestIdKey, Guid.NewGuid());

            //Create and start global stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            requestStore.Set(Constants.GlobalStopwatchKey, stopwatch);
        }

        //Todo: Make sure request has begun?
        //Todo: Add PRG support
        //Todo: Add Name to data protocol
        //Todo: Process MetaData
        public void EndRequest()
        {
            //TODO: stop glimpse timer (if needed?)

            var mode = GetRuntimePolicy(RuntimeEvent.EndRequest);
            if (mode == RuntimePolicy.Off) return;

            var encoder = Configuration.HtmlEncoder;
            var frameworkProvider = Configuration.FrameworkProvider;
            var serializer = Configuration.Serializer;
            //TODO: Store data and name
            var pluginResults = PluginResultsStore.ToDictionary(item => item.Key,
                                                                item => serializer.Serialize(item.Value));
            var requestMetadata = frameworkProvider.RequestMetadata;
            var requestStore = frameworkProvider.HttpRequestStore;
            var resourceEndpoint = Configuration.ResourceEndpoint;
            Guid requestId;

            //TODO: Sanitize JSON
            //TODO: Structured layout support

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
            //TODO: Finish implementing persistance store
            Configuration.PersistanceStore.Save(metadata);

            //TODO: Filter out requests that should not have the ID header
            //TODO: Check glimpse mode first
            frameworkProvider.SetHttpResponseHeader(Constants.HttpHeader, requestId.ToString());

            var dataPath =
                encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUrl("data.js", Version,
                                                                         new Dictionary<string, string>
                                                                             {{"id", requestId.ToString()}}));
            var clientPath = encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUrl("client.js", Version));

            //var dataPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId());
            //var clientPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("client.js"));

            var html =
                string.Format(
                    @"<script type='text/javascript' id='glimpseData' src='{0}'></script><script type='text/javascript' id='glimpseClient' src='{1}'></script></body>",
                    dataPath, clientPath);

            //TODO: Only if this isn't an Ajax request/Body manipulation is allowed
            frameworkProvider.InjectHttpResponseBody(html);
        }

        public void ExecuteResource(string resourceName)
        {
            ExecuteResource(resourceName, new Dictionary<string, string>());
        }

        public void ExecuteResource(string resourceName, IDictionary<string, string> parameters)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(resourceName), "resourceName");

            var mode = GetRuntimePolicy(RuntimeEvent.ExecuteResource);
            if (mode == RuntimePolicy.Off) return;

            var logger = Configuration.Logger;
            ResourceResult result;

            var resources =
                Configuration.Resources.Where(
                    r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

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
            catch (Exception ex)
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
            var mode = GetRuntimePolicy(RuntimeEvent.ExecuteTabs);
            if (mode == RuntimePolicy.Off) return;

            var runtimeContext = Configuration.FrameworkProvider.RuntimeContext;
            var frameworkProviderRuntimeContextType = runtimeContext.GetType();

            //Only use tabs that either don't specify a specific context type, or have a context type that matches the current framework provider's.
            var runtimePlugins =
                Configuration.Tabs.Where(
                    p =>
                    p.Metadata.RequestContextType == null ||
                    frameworkProviderRuntimeContextType.IsSubclassOf(p.Metadata.RequestContextType) ||
                    p.Metadata.RequestContextType == frameworkProviderRuntimeContextType);

            var supportedRuntimePlugins = runtimePlugins.Where(p => p.Metadata.LifeCycleSupport.HasFlag(support));
            var pluginResultsStore = PluginResultsStore;
            var logger = Configuration.Logger;


            //Create storage space for plugins to access
            var pluginStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());

            //Create ServiceLocator valid for this request
            var tabContext = new TabContext(runtimeContext, pluginStore, Configuration.PipelineInspectors);


            foreach (var plugin in supportedRuntimePlugins)
            {
                var key = plugin.Value.GetType().FullName;
                try
                {
                    if (pluginResultsStore.ContainsKey(key))
                        pluginResultsStore[key] = plugin.Value.GetData(tabContext);
                    else
                        pluginResultsStore.Add(key, plugin.Value.GetData(tabContext));
                }
                catch (Exception exception)
                {
                    logger.Error(string.Format(Resources.ExecuteTabError, key), exception);
                }
            }
        }

        public bool Initialize()
        {
            var logger = Configuration.Logger;
            var mode = GetRuntimePolicy(RuntimeEvent.Initialize);
            if (mode == RuntimePolicy.Off) return false;

            //TODO: pass valid context into pipelineInspector
            var tabsThatRequireSetup = Configuration.Tabs.Where(p => p.Value is IGlimpseTabSetup).Select(p => p.Value);
            foreach (IGlimpseTabSetup tab in tabsThatRequireSetup)
            {
                try
                {
                    tab.Setup();
                }
                catch (Exception exception)
                {
                    logger.Error(string.Format(Resources.InitializeTabError, tab.GetType()), exception);
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
                        string.Format(Resources.InitializePipelineInspectorError, pipelineInspector.GetType()),
                        exception);
                }
            }

            IsInitialized = true;

            return mode != RuntimePolicy.Off;
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

            if (configuration.RuntimePolicies.Discoverability.AutoDiscover)
                configuration.RuntimePolicies.Discoverability.Discover();

            Configuration = configuration;
        }

        private RuntimePolicy GetRuntimePolicy(RuntimeEvent runtimeEvent)
        {
            var frameworkProvider = Configuration.FrameworkProvider;
            var requestStore = frameworkProvider.HttpRequestStore;
            var result = requestStore.Contains(Constants.RuntimePermissionsKey)
                             ? requestStore.Get<RuntimePolicy>(Constants.RuntimePermissionsKey)
                             : Configuration.BasePolicy;

            if (result != RuntimePolicy.Off)
            {
                var validators =
                    Configuration.RuntimePolicies.Where(
                        v => !v.Metadata.RuntimeEvent.HasValue || v.Metadata.RuntimeEvent.Value.HasFlag(runtimeEvent));

                var policyContext = new RuntimePolicyContext(frameworkProvider.RequestMetadata, Configuration.Logger,frameworkProvider.RuntimeContext);
                foreach (var validator in validators)
                {
                    //TODO: Handle exceptions from policy
                    var mode = validator.Value.Execute(policyContext);

                    //Only use the lowest level allowed for the request
                    if (mode < result) result = mode;
                }
            }

            requestStore.Set(Constants.RuntimePermissionsKey, result);
            return result;
        }
    }
}