using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The default implementation of <see cref="IMessageBroker"/> which supports subscribing to messages based on types, base types and interfaces.
    /// </summary>
    public class MessageBroker : IMessageBroker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBroker" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if <paramref name="logger"/> is <c>null</c>.</exception>
        public MessageBroker(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Subscriptions = new Dictionary<Type, List<Subscriber>>();
            Logger = logger;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        /// <value>
        /// The subscriptions.
        /// </value>
        internal IDictionary<Type, List<Subscriber>> Subscriptions { get; set; }

        /// <summary>
        /// Publishes the specified message using the type as the topic.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="message">The message.</param>
        public void Publish<T>(T message)
        {
            foreach (var subscriptions in Subscriptions)
            {
                if (subscriptions.Key.IsInstanceOfType(message))
                {
                    foreach (var subscriber in subscriptions.Value)
                    {
                        try
                        {
                            subscriber.Execute(message);
                        }
                        catch (Exception exception)
                        {
                            Logger.Error("Exception calling subscriber with message of type '{0}'.", exception, typeof(T));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Subscribes the specified action to the Type specified.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>
        /// A subscription Id, which should be retained in order to unsubscribe.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throws an exception if <paramref name="action"/> is <c>null</c>.</exception>
        public Guid Subscribe<T>(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var subscriptions = GetSubscriptions(typeof(T));

            var subscriptionId = Guid.NewGuid();
            subscriptions.Add(new Subscriber<T>(action, subscriptionId));

            Logger.Debug(Resources.MessageBrokerSubscribe, action.Method.Name, action.Method.DeclaringType, typeof(T));

            return subscriptionId;
        }

        /// <summary>
        /// Unsubscribes the listener from the specified subscription id.
        /// </summary>
        /// <typeparam name="T">Type of the message</typeparam>
        /// <param name="subscriptionId">The subscription id.</param>
        public void Unsubscribe<T>(Guid subscriptionId)
        {
            var subscriptions = GetSubscriptions(typeof(T));
            subscriptions.RemoveAll(i => i.SubscriptionId == subscriptionId);
        }

        private List<Subscriber> GetSubscriptions(Type type)
        {
            if (Subscriptions.ContainsKey(type))
            {
                return Subscriptions[type];
            }

            var result = new List<Subscriber>();
            Subscriptions.Add(type, result);

            return result;
        }
    }
}