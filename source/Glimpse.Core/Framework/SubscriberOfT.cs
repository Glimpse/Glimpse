using System;

namespace Glimpse.Core.Framework
{
    internal class Subscriber<T> : Subscriber
    {
        public Subscriber(Action<T> action, Guid subscriptionId) : base(subscriptionId)
        {
            Action = action;
        }

        private Action<T> Action { get; set; }

        public override void Execute(object message)
        {
            Action((T)message);
        }
    }
}