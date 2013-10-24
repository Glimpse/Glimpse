using System;
using Glimpse.Core.Message;

namespace Glimpse.WebForms.Inspector
{
    public class PageLifeCycleMessage : MessageBase, ITimelineMessage
    {
        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }

        public TimeSpan FromLast { get; set; }
    }
}