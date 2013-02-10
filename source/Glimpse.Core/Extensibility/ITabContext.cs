namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the context which tabs use when generating the 
    /// content will be returned. 
    /// </summary>
    public interface ITabContext : IContext
    {
        /// <summary>
        /// Gets access to the tab store. This is where content can be 
        /// stored within the context of each request.
        /// </summary>
        /// <value>The tab store.</value>
        IDataStore TabStore { get; }

        /// <summary>
        /// Gets access to the message broker. This broker can be used to 
        /// access messages that are published over various topics during 
        /// a given request.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <typeparam name="T">Type of result that is expected.</typeparam>
        /// <returns>The request context that is being used.</returns>
        T GetRequestContext<T>() where T : class;
    }
}
