using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The implementation of <see cref="ITabSetupContext"/> used by the <c>Setup</c> method of <see cref="ITabSetup"/>.
    /// </summary>
    public class TabSetupContext : ITabSetupContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabSetupContext" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="messageBroker">The message broker.</param>
        /// <param name="tabStoreStrategy">The tab store strategy.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if any parameter is <c>null</c>.</exception>
        public TabSetupContext(ILogger logger, IMessageBroker messageBroker, Func<IDataStore> tabStoreStrategy)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (messageBroker == null)
            {
                throw new ArgumentNullException("messageBroker");
            }

            if (tabStoreStrategy == null)
            {
                throw new ArgumentNullException("tabStoreStrategy");
            }
            
            Logger = logger;
            MessageBroker = messageBroker;
            TabStoreStrategy = tabStoreStrategy;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the message broker.
        /// </summary>
        /// <value>
        /// The message broker.
        /// </value>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the tab store strategy.
        /// </summary>
        /// <value>
        /// The tab store strategy.
        /// </value>
        private Func<IDataStore> TabStoreStrategy { get; set; }

        /// <summary>
        /// Gets the tab store.
        /// </summary>
        /// <returns>
        /// Data store that can be used.
        /// </returns>
        public IDataStore GetTabStore()
        {
            return TabStoreStrategy();
        }
    }
}