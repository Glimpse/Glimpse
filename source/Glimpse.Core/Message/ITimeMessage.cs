using System;

namespace Glimpse.Core.Message
{
    public interface ITimeMessage : IMessage
    {
        double Offset { get; }

        double Duration { get; }

        DateTime StartTime { get; }
    }
}