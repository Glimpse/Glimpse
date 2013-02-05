using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The <see cref="ITabContext"/> implementation used by the <c>GetData</c> method of <see cref="ITab"/>.
    /// </summary>
    public class TabContext : ITabContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabContext" /> class.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="tabStore">The tab store.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messageBroker">The message broker.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if any parameter if <c>null</c>.</exception>
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

        /// <summary>
        /// Gets or sets access to the tab store. This is where content can be
        /// stored within the context of each request.
        /// </summary>
        /// <value>
        /// The tab store.
        /// </value>
        public IDataStore TabStore { get; private set; }

        /// <summary>
        /// Gets or sets access to the message broker. This broker can be used to
        /// access messages that are published over various topics during
        /// a given request.
        /// </summary>
        /// <value>
        /// The message broker.
        /// </value>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }
        
        private object RequestContext { get; set; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <typeparam name="T">Type of result that is expected.</typeparam>
        /// <returns>
        /// The request context that is being used.
        /// </returns>
        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }
    }
}