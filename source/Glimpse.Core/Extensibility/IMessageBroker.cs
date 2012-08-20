using System;

namespace Glimpse.Core.Extensibility
{
    public interface IMessageBroker
    {
        void Publish<T>(T message);
        Guid Subscribe<T>(Action<T> action);
        void Unsubscribe<T>(Guid subscriptionId);
    }
}