using System.Collections.Generic;

namespace Glimpse.Core.Message
{
    public interface ITimerResultMessage : ITimeMessage
    {
        string EventName { get; }
        
        string EventCategory { get; }

        string EventSubText { get; }

        void BuildDetails(IDictionary<string, object> details);
    }
}