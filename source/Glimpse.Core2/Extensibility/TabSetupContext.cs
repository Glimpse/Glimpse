using System;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    public class TabSetupContext:ITabSetupContext
    {
        public TabSetupContext(ILogger logger, IMessageBroker messageBroker, Func<IDataStore> tabStoreStrategy)
        {
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(messageBroker != null, "messageBroker");
            Contract.Requires<ArgumentNullException>(tabStoreStrategy != null, "tabStoreStrategy");

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