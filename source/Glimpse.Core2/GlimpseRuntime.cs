using System;
using System.Collections.Generic;
using System.Diagnostics;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class GlimpseRuntime
    {
        private GlimpseConfiguration Configuration { get; set; }

        public GlimpseRuntime(GlimpseConfiguration configuration)
        {
            UpdateConfiguration(configuration);
        }

        public void UpdateConfiguration(GlimpseConfiguration configuration)
        {
            if (configuration.Plugins.Discoverability.AutoDiscover)
                configuration.Plugins.Discoverability.Discover();

            if (configuration.PipelineModifiers.Discoverability.AutoDiscover)
                configuration.PipelineModifiers.Discoverability.Discover();

            Configuration = configuration;
        }

        public void BeginRequest()
        {
            var runtimeContext = Configuration.FrameworkProvider.RuntimeContext;
            var requestStore = Configuration.FrameworkProvider.HttpRequestStore;
            
            //Create storage space for plugins to access
            var pluginStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
            requestStore.Set(pluginStore);

            //Create ServiceLocator valid for this request
            requestStore.Set(new GlimpseServiceLocator(runtimeContext, pluginStore, Configuration.PipelineModifiers));

            //Give Request an ID
            requestStore.Set(Guid.NewGuid());

            //Create and start global stopwatch
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            requestStore.Set(stopwatch);
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
    }
}