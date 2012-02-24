namespace Glimpse.Core2.Extensibility
{
    public interface ITabContext:IContext
    {
        T GetRequestContext<T>() where T:class;
        IDataStore PluginStore { get; }
        IMessageBroker MessageBroker { get; }
    }
}
