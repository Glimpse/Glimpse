using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class GlimpseRuntime
    {
        private GlimpseConfiguration Configuration { get; set; }
        public string Version { get; private set; }

        public GlimpseRuntime(GlimpseConfiguration configuration)
        {
            //Version is in major.minor.build format to support http://semver.org/
            Version = GetType().Assembly.GetName().Version.ToString(3);
            UpdateConfiguration(configuration);
        }

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

        public void Initialize()
        {
            //TODO: Add in validation checks
            var pluginsThatRequireSetup = Configuration.Tabs.Where(p => p.Value is IGlimpseTabSetup).Select(p=>p.Value);
            foreach (IGlimpseTabSetup plugin in pluginsThatRequireSetup)
            {
                try
                {
                    plugin.Setup();
                }
                catch (Exception exception)
                {
                    //TODO: Add logging
                }
            }

            foreach (var pipelineModifier in Configuration.PipelineInspectors)
            {
                try
                {
                    pipelineModifier.Setup();
                }
                catch (Exception exception)
                {
                    //TODO: Add logging
                }
            }
        }

        public void BeginRequest()
        {
            var frameworkProvider = Configuration.FrameworkProvider;
            var runtimeContext = frameworkProvider.RuntimeContext;
            var requestStore = frameworkProvider.HttpRequestStore;
            
            //Create storage space for plugins to access
            var pluginStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
            requestStore.Set(pluginStore);

            //Create ServiceLocator valid for this request
            requestStore.Set(new GlimpseServiceLocator(runtimeContext, pluginStore, Configuration.PipelineInspectors));

            //Give Request an ID
            requestStore.Set(Guid.NewGuid());

            //Create and start global stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            requestStore.Set(stopwatch);
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

            foreach (var plugin in supportedRuntimePlugins)
            {
                try
                {
                    var key = plugin.Value.GetType().FullName;
                    ResultsStore.Add(key, plugin.Value.GetData(ServiceLocator));
                }
                catch (Exception exception)
                {
                    //TODO: Add in logging
                }
            }
        }

        public void EndRequest()
        {
            var serializer = Configuration.Serializer;
            var frameworkProvider = Configuration.FrameworkProvider;
            var requestStore = frameworkProvider.HttpRequestStore;
            var requestMetadata = frameworkProvider.RequestMetadata;
            var pluginData = ResultsStore.ToDictionary(item => item.Key, item => serializer.Serialize(item.Value));
            var requestId = requestStore.Get<Guid>();
            var encoder = Configuration.HtmlEncoder;
            var resourceEndpoint = Configuration.ResourceEndpoint;

            var metadata = new GlimpseMetadata(requestId, requestMetadata, pluginData);

            //TODO: Handle exceptions
            Configuration.PersistanceStore.Save(metadata);

            //TODO: Filter out requests that should not have the ID header
            frameworkProvider.SetHttpResponseHeader("X-Glimpse-RequestID", requestId.ToString());

            var dataPath = encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUrl("data.js", Version, new Dictionary<string, string>{{"id", requestId.ToString()}}));
            var clientPath = encoder.HtmlAttributeEncode(resourceEndpoint.GenerateUrl("client.js", Version));
            
            //var dataPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("data.js") + "&id=" + Context.GetGlimpseRequestId());
            //var clientPath = HttpUtility.HtmlAttributeEncode(Context.GlimpseResourcePath("client.js"));

            var html = string.Format(@"<script type='text/javascript' id='glimpseData' src='{0}'></script><script type='text/javascript' id='glimpseClient' src='{1}'></script></body>", dataPath, clientPath);

            frameworkProvider.InjectHttpResponseBody(html);

        }

        public IServiceLocator ServiceLocator
        {
            get
            {
                var result = Configuration.FrameworkProvider.HttpRequestStore.Get<GlimpseServiceLocator>();

                if (result == null)
                    throw new Exception("Must BeginRequest() first"); //TODO: User better exceptions

                return result;
            }
        }

        private IDictionary<string, object> ResultsStore
        {
            get
            {
                var requestStore = Configuration.FrameworkProvider.HttpRequestStore;
                var result = requestStore.Get<IDictionary<string, object>>("__GlimpseResults");

                if (result == null)
                {
                    result = new Dictionary<string, object>();
                    requestStore.Set("__GlimpseResults", result);
                }

                return result;
            }
        }
    }
}