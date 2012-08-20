using System;

namespace Glimpse.Core.Framework
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