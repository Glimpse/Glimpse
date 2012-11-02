using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class MessageBrokerExtensions
    {
        public static void PublishMany<T1, T2>(this IMessageBroker messageBroker, T1 message1, T2 message2)
        {
            messageBroker.Publish(message1);
            messageBroker.Publish(message2);
        }
    }
}
