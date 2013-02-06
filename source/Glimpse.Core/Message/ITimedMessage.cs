using System;

namespace Glimpse.Core.Message
{
    public interface ITimedMessage : IMessage
    {
        TimeSpan Offset { get; }

        TimeSpan Duration { get; }

        DateTime StartTime { get; }
    }
}