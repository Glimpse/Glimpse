namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the context which is used during tab setup.
    /// </summary>
    public interface ITabSetupContext : IContext
    {
        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; }

        /// <summary>
        /// Gets the tab store.
        /// </summary>
        /// <returns>Data store that can be used.</returns>
        IDataStore GetTabStore();
    }
}