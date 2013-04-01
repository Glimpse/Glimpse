using System;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public class ConnectionClosedMessage : AdoMessage, ITimelineMessage 
    {
        public ConnectionClosedMessage(Guid connectionId) 
            : base(connectionId)
        {
        }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }
    }
}