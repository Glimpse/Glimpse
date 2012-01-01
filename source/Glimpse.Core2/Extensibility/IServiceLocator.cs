namespace Glimpse.Core2.Extensibility
{
    public interface IServiceLocator
    {
        object RequestContext { get; }
        T GetPipelineInspector<T>() where T:class, IGlimpsePipelineInspector;
        IDataStore PluginStore { get; }
    }
}
