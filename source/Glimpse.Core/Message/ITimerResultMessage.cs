using System;
using System.Collections.Generic;

namespace Glimpse.Core.Message
{
    public interface ITimerResultMessage : IMessage
    {
        string EventName { get; }
        
        string EventCategory { get; }

        string EventSubText { get; }

        double Offset { get; }

        double Duration { get; }

        DateTime StartTime { get; }

        void BuildDetails(IDictionary<string, object> details);
    }
}