using System;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public class CommandDurationAndRowCountMessage : AdoCommandMessage, ITimelineMessage
    {
        public CommandDurationAndRowCountMessage(Guid connectionId, Guid commandId, long? recordsAffected)
            : base(connectionId, commandId)
        {
            CommandId = commandId; 
            RecordsAffected = recordsAffected;
        } 

        public long? RecordsAffected { get; protected set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }
    }
}