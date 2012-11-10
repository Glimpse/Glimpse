using System;

namespace Glimpse.Core.Message
{
    public interface ITimerResultMessage : IMessage
    {
        string EventName { get; }
        
        string EventCategory { get; }

        double Offset { get; }

        double Duration { get; }
    }
}