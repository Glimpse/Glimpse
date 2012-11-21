using System;

namespace Glimpse.Core.Extensibility
{
    public class TabSetupContext : ITabSetupContext
    {
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

        public ILogger Logger { get; set; }

        public IMessageBroker MessageBroker { get; set; }

        private Func<IDataStore> TabStoreStrategy { get; set; }

        public IDataStore GetTabStore()
        {
            return TabStoreStrategy();
        }
    }
}