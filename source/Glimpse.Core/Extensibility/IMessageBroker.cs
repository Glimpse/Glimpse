using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of the Message Broker that will used by the system.
    /// </summary>
    public interface IMessageBroker
    {
        /// <summary>
        /// Publishes the specified message using the type as the topic.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="message">The message.</param>
        void Publish<T>(T message);

        /// <summary>
        /// Subscribes the specified action to the Type specified.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>A subscription Id, which should be retained in order to unsubscribe.</returns>
        Guid Subscribe<T>(Action<T> action);

        /// <summary>
        /// Unsubscribes the listener from the specified subscription id.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="subscriptionId">The subscription id.</param>
        void Unsubscribe<T>(Guid subscriptionId);
    }
}