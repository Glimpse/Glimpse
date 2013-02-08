using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public interface ITimedMessage : IMessage
    {
        TimeSpan Offset { get; set; }

        TimeSpan Duration { get; set; }

        DateTime StartTime { get; set; }
    }

    public static class TimedMessageExtension
    {
        public static T AsTimedMessage<T>(this T message, TimerResult timerResult)
            where T : ITimedMessage
        {
            message.Offset = timerResult.Offset;  
            message.Duration = timerResult.Duration; 
            message.StartTime = timerResult.StartTime; 

            return message;
        }
    }
}