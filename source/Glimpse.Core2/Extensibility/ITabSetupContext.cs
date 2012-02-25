namespace Glimpse.Core2.Extensibility
{
    public interface ITabSetupContext:IContext
    {
        IMessageBroker MessageBroker { get; }
        IDataStore GetTabStore();
    }
}