using System;

namespace Glimpse.Core.Extensibility
{
    public class TabContext : ITabContext
    {
        public TabContext(object requestContext, IDataStore tabStore, ILogger logger, IMessageBroker messageBroker)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            if (tabStore == null)
            {
                throw new ArgumentNullException("tabStore");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (messageBroker == null)
            {
                throw new ArgumentNullException("messageBroker");
            }

            RequestContext = requestContext;
            TabStore = tabStore;
            Logger = logger;
            MessageBroker = messageBroker;
        }

        public IDataStore TabStore { get; private set; }

        public IMessageBroker MessageBroker { get; set; }

        public ILogger Logger { get; set; }
        
        private object RequestContext { get; set; }

        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }
    }
}