namespace Glimpse.Core.Extensibility
{
    public interface ITabContext : IContext
    {
        IDataStore TabStore { get; }
        
        IMessageBroker MessageBroker { get; }
        
        T GetRequestContext<T>() where T : class;
    }
}
