using System;
using System.Reflection; 
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class TimeMessage : MessageBase
    {
        public TimeMessage(TimerResult timerResult, Type executedType, MethodInfo executedMethod)
            : base(executedType, executedMethod)
        { 
            Result = timerResult;
        }

        public TimeSpan Offset
        {
            get { return Result.Offset; }
        }

        public TimeSpan Duration
        {
            get { return Result.Duration; }
        }

        public DateTime StartTime
        {
            get { return Result.StartTime; }
        }

        private TimerResult Result { get; set; }
    }
}