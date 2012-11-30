using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimelineMessage : TimeMessage, ITimerResultMessage
    {
        public TimelineMessage(TimerResult timerResult, string eventName = null, string eventCategory = null) 
            : base(timerResult)
        {
            EventName = eventName;
            EventCategory = eventCategory;
        }

        public string EventName { get; protected set; }

        public string EventCategory { get; protected set; }

        public string EventSubText { get; protected set; }
        
        public virtual void BuildDetails(IDictionary<string, object> details)
        { 
        }
    }
}