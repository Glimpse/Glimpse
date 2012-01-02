using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var result = Configuration.FrameworkProvider.HttpRequestStore.Get<GlimpseServiceLocator>(Constants.ServiceLocatorKey);

                if (result == null)
                    throw new MethodAccessException(Resources.OutOfOrderRuntimeMethodCall);

                return result;
            }
        }

        public string Version { get; private set; }



        //TODO: Make sure runtime has been init'ed
        public void BeginRequest()
        {
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
            var encoder = Configuration.HtmlEncoder;
            var frameworkProvider = Configuration.FrameworkProvider;
            var serializer = Configuration.Serializer;
            var pluginResults = PluginResultsStore.ToDictionary(item => item.Key, item => serializer.Serialize(item.Value));
            var requestMetadata = frameworkProvider.RequestMetadata;
            var requestStore = frameworkProvider.HttpRequestStore;
            var resourceEndpoint = Configuration.ResourceEndpoint;
            Guid requestId;

            try
            {
                requestId = requestStore.Get<Guid>(Constants.RequestIdKey);
            }
            catch(NullReferenceException ex)
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
        
        public void ExecutePlugins()
        {
            ExecutePlugins(LifeCycleSupport.EndRequest);
        }

        public void ExecutePlugins(LifeCycleSupport support)
        {
            //Only use tabs that either don't specify a specific context type, or have a context type that matches the current framework provider's.
            var runtimePlugins = Configuration.Tabs.Where(p=>p.Metadata.RequestContextType == null || p.Metadata.RequestContextType == Configuration.FrameworkProvider.RuntimeContextType);
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
                   logger.Error(string.Format(Resources.ExecutePluginError, key), exception);
                }
            }
        }

        //Todo: Set an "Init'ed" bit
        public void Initialize()
        {
            var logger = Configuration.Logger;

            //TODO: Add in request validation checks
            var tabsThatRequireSetup = Configuration.Tabs.Where(p => p.Value is IGlimpseTabSetup).Select(p=>p.Value);
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
                    logger.Error(string.Format(Resources.InitializePipelineInspectorError, pipelineInspector.GetType().FullName), exception);
                }
            }
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
    }
}