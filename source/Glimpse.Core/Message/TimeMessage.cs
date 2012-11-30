using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimeMessage : MessageBase
    {
        public TimeMessage(TimerResult timerResult)
        { 
            this.Result = timerResult;
        }

        public double Offset
        {
            get { return this.Result.Offset; }
        }

        public double Duration
        {
            get { return this.Result.Duration; }
        }

        public DateTime StartTime
        {
            get { return this.Result.StartTime; }
        }

        private TimerResult Result { get; set; }
    }
}