using System;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public class CommandDurationAndRowCountMessage : AdoCommandMessage, ITimelineMessage
    {
        public CommandDurationAndRowCountMessage(Guid connectionId, Guid commandId, long? recordsAffected)
            : this(connectionId, commandId, recordsAffected, false)
        { }

        public CommandDurationAndRowCountMessage(Guid connectionId, Guid commandId, long? recordsAffected, bool isAsync)
            : base(connectionId, commandId)
        {
            CommandId = commandId;
            RecordsAffected = recordsAffected;
            IsAsync = isAsync;
        } 

        public long? RecordsAffected { get; protected set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }

        public bool IsAsync { get; set; }
    }
}