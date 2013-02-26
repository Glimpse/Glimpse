using System.Collections.Generic;

namespace Glimpse.Core.Message
{
    public interface ITimelineMessage : ITimedMessage
    {
        string EventName { get; set; }

        TimelineCategory EventCategory { get; set; }

        string EventSubText { get; set; }
    }
     
    public static class TimelineMessageExtension
    {
        public static T AsTimelineMessage<T>(this T message, string eventName, TimelineCategory eventCategory, string eventSubText = null)
            where T : ITimelineMessage
        {
            message.EventName = eventName;
            message.EventCategory = eventCategory;
            message.EventSubText = eventSubText;

            return message;
        }

        public static T AsTimelineMessage<T>(this T message, TimelineCategory eventCategory, string eventSubText = null)
            where T : ITimelineMessage
        { 
            message.AsTimelineMessage(string.Empty, eventCategory, eventSubText);

            return message;
        }
    }
}