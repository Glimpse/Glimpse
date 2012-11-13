using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimelineMessage : MessageBase, ITimerResultMessage
    {
        public TimelineMessage(TimerResult timerResult, string eventName = null, string eventCategory = null) 
        {
            Result = timerResult;
            EventName = eventName;
            EventCategory = eventCategory;
        }

        public string EventName { get; protected set; }

        public string EventCategory { get; protected set; }
        
        public double Offset
        {
            get { return Result.Offset; }
        }

        public double Duration
        {
            get { return Result.Duration; }
        }

        public DateTime StartTime
        {
            get { return Result.StartTime; }
        }

        private TimerResult Result { get; set; }

        public virtual void BuildEvent(ITimelineEvent timelineEvent)
        {
            timelineEvent.Title = EventName;
            timelineEvent.Category = EventCategory;
            timelineEvent.Duration = Duration;
            timelineEvent.StartPoint = Offset;
            timelineEvent.StartTime = StartTime;
        }
    }
}