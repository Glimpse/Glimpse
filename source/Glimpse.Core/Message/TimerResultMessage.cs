using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimerResultMessage : MessageBase
    {
        public TimerResultMessage(TimerResult timerResult, string eventName, string eventCategory)
        {
            Result = timerResult;
            EventName = eventName;
            EventCategory = eventCategory;
        }

        public string EventName { get; set; }
        
        public string EventCategory { get; set; }
        
        public long Offset
        {
            get { return Result.Offset; }
        }

        public TimeSpan Duration
        {
            get { return Result.Duration; }
        }

        private TimerResult Result { get; set; }
    }
}