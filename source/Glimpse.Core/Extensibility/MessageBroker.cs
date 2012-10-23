using System;
using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    public class MessageBroker : IMessageBroker
    {
        public MessageBroker(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Subscriptions = new Dictionary<Type, List<Subscriber>>();
            Logger = logger;
        }

        public ILogger Logger { get; set; }

        internal IDictionary<Type, List<Subscriber>> Subscriptions { get; set; }

        public void Publish<T>(T message)
        {
            foreach (var subscriber in GetSubscriptions(typeof(T)))
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