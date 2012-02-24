using System;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    public class TabContext:ITabContext
    {

        public TabContext(object requestContext, IDataStore pluginStore, ILogger logger, IMessageBroker messageBroker)
        {
            Contract.Requires<ArgumentNullException>(requestContext != null, "requestContext");
            Contract.Requires<ArgumentNullException>(pluginStore != null, "pluginStore");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(messageBroker != null, "messageBroker");

            RequestContext = requestContext;
            PluginStore = pluginStore;
            Logger = logger;
            MessageBroker = messageBroker;
        }

        public IDataStore PluginStore { get; private set; }

        public IMessageBroker MessageBroker { get; set; }

        private object RequestContext { get; set; }

        public ILogger Logger { get; set; }

        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }
    }
}