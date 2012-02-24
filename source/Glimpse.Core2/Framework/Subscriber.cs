using System;

namespace Glimpse.Core2.Framework
{
    internal abstract class Subscriber
    {
        protected Subscriber(Guid subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; set; }
        public abstract void Execute(object message);
    }
}