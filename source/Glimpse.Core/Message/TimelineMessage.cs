using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimelineMessage : MessageBase, ITimerResultMessage
    {
        public TimelineMessage(TimerResult timerResult)
        {
            Result = timerResult;
        }

        public TimelineMessage(TimerResult timerResult, string eventName, string eventCategory)
            : this(timerResult)
        { 
            EventName = eventName;
            EventCategory = eventCategory;
        }

        public string EventName { get; set; }
        
        public string EventCategory { get; set; }
        
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