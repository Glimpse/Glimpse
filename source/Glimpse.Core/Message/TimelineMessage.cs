using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimelineMessage : MessageBase, ITimelineMessage
    {
        public TimelineMessage(TimerResult timerResult, string eventName, string eventSubText)
            : this(timerResult, eventName, eventSubText, new TimelineCategoryItem("GlimpseTimeline.Capture", "#3c454f", "#eee"))
        {
        }


        public TimelineMessage(TimerResult timerResult, string eventName, string eventSubText, TimelineCategoryItem eventCategory)
        {
            Offset = timerResult.Offset;
            Duration = timerResult.Duration;
            StartTime = timerResult.StartTime;
            EventName = eventName;
            EventSubText = eventSubText;
            EventCategory = eventCategory;
        }

        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }
    }
}