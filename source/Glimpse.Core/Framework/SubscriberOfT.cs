using System;

namespace Glimpse.Core.Framework
{
    internal class Subscriber<T> : Subscriber
    {
        private Action<T> Action { get; set; }

        public Subscriber(Action<T> action, Guid subscriptionId) : base(subscriptionId)
        {
            Action = action;
        }

        public override void Execute(object message)
        {
            Action((T) message);
        }
    }
}