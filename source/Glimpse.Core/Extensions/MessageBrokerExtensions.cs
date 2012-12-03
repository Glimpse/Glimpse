using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Core.Extensions
{
    public static class MessageBrokerExtensions
    {
        public static void PublishMany<T1, T2>(this IMessageBroker messageBroker, T1 message1, T2 message2) where T1 : MessageBase where T2 : MessageBase
        {
            messageBroker.Publish(message1);
            messageBroker.Publish(message2);
        }
    }
}
