namespace Glimpse.Core2.Extensibility
{
    //TODO: Should we call this RequestContext? or GlimpseContext?
    public interface IServiceLocator
    {
        //TODO: What about GetRequestContect<T> where T:class?
        object RequestContext { get; }
        T GetPipelineInspector<T>() where T:class, IGlimpsePipelineInspector;
        IDataStore PluginStore { get; }
        //TODO: Do we say which kind of 
    }
}
