using System;

namespace Glimpse.Core.Extensibility
{
    public interface IMessageBroker
    {
        void Publish<T>(T message) where T : MessageBase;

        Guid Subscribe<T>(Action<T> action) where T : MessageBase;

        void Unsubscribe<T>(Guid subscriptionId) where T : MessageBase;
    }
}