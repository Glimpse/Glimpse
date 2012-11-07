using System;

namespace Glimpse.Core.Message
{
    public interface ITimerResultMessage
    {
        string EventName { get; }
        
        string EventCategory { get; }
        
        long Offset { get; }

        TimeSpan Duration { get; }
    }
}