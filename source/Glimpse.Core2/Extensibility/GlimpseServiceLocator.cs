using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Glimpse.Core2.Extensibility
{
    public class GlimpseServiceLocator:IServiceLocator
    {

        public GlimpseServiceLocator(object requestContext, IDataStore pluginStore, GlimpseCollection<IGlimpsePipelineInspector> pipelineInspectors)
        {
            Contract.Requires<ArgumentNullException>(requestContext != null, "requestContext");
            Contract.Requires<ArgumentNullException>(pluginStore != null, "pluginStore");
            Contract.Requires<ArgumentNullException>(pipelineInspectors != null, "pipelineInspectors");

            RequestContext = requestContext;
            PluginStore = pluginStore;
            PipelineInspectors = pipelineInspectors;
        }



        private GlimpseCollection<IGlimpsePipelineInspector> PipelineInspectors { get; set; }

        public IDataStore PluginStore { get; private set; }

        public object RequestContext { get; private set; }



        //TODO: Provide a non generic implementation? GetPipelineInspector(Type type)?
        public T GetPipelineInspector<T>() where T : class, IGlimpsePipelineInspector
        {
            return PipelineInspectors.FirstOrDefault(pm => pm.GetType() == typeof (T)) as T;
        }
    }
}