using System;

namespace Glimpse.Core.Message
{
    public interface ITimeMessage : IMessage
    {
        TimeSpan Offset { get; }

        TimeSpan Duration { get; }

        DateTime StartTime { get; }
    }
}