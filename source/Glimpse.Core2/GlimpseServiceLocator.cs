using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class GlimpseServiceLocator:IServiceLocator
    {
        public GlimpseServiceLocator(object requestContext, IDataStore pluginStore, GlimpseCollection<IGlimpsePipelineInspector> pipelineModifiers)
        {
            RequestContext = requestContext;
            PluginStore = pluginStore;
            PipelineModifiers = pipelineModifiers;
        }

        public object RequestContext { get; private set; }
        public IDataStore PluginStore { get; private set; }
        private GlimpseCollection<IGlimpsePipelineInspector> PipelineModifiers { get; set; }

        public T GetPipelineModifier<T>() where T : class, IGlimpsePipelineInspector
        {
            return PipelineModifiers.Where(pm => pm.GetType() == typeof (T)).FirstOrDefault() as T;
        }
    }
}