namespace Glimpse.Core2.Extensibility
{
    public interface ITabContext
    {
        T GetRequestContext<T>() where T:class;
        T GetPipelineInspector<T>() where T:class, IPipelineInspector;
        IDataStore PluginStore { get; }
    }
}
