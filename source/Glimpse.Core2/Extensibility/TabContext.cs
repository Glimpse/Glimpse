using System;

namespace Glimpse.Core2.Extensibility
{
    public class TabContext:ITabContext
    {

        public TabContext(object requestContext, IDataStore pluginStore, ILogger logger, IMessageBroker messageBroker)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");
            if (pluginStore == null) throw new ArgumentNullException("pluginStore");
            if (logger == null) throw new ArgumentNullException("logger");
            if (messageBroker == null) throw new ArgumentNullException("messageBroker");

            RequestContext = requestContext;
            TabStore = pluginStore;
            Logger = logger;
            MessageBroker = messageBroker;
        }

        public IDataStore TabStore { get; private set; }

        public IMessageBroker MessageBroker { get; set; }

        private object RequestContext { get; set; }

        public ILogger Logger { get; set; }

        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }
    }
}