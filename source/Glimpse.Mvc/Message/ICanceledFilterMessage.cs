namespace Glimpse.Mvc.Message
{
    public interface ICanceledBasedFilterMessage : IFilterMessage
    {
        bool Canceled { get; set; }
    }

    public static class CanceledBasedFilterMessageExtension
    {
        public static T AsCanceledFilterMessage<T>(this T message, bool canceled)
            where T : ICanceledBasedFilterMessage
        {
            message.Canceled = canceled;

            return message;
        }
    }
}