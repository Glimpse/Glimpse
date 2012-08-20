namespace Glimpse.Core.Extensibility
{
    public interface ITabSetupContext:IContext
    {
        IMessageBroker MessageBroker { get; }
        IDataStore GetTabStore();
    }
}