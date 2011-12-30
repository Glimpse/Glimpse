namespace Glimpse.Core2.Extensibility
{
    public interface IServiceLocator
    {
        object RequestContext { get; }
        T GetPipelineModifier<T>() where T:class, IGlimpsePipelineInspector;
        IDataStore PluginStore { get; }
    }
}
