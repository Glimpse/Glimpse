using System.Collections.Generic;

namespace Glimpse.Core.Message
{
    public interface ITimelineMessage : ITimedMessage
    {
        string EventName { get; }
        
        TimelineCategory EventCategory { get; }

        string EventSubText { get; }

        void BuildDetails(IDictionary<string, object> details);
    }
}