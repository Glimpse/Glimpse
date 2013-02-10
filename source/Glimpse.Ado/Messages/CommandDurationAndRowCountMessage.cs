using System;

namespace Glimpse.Ado.Messages
{
    public class CommandDurationAndRowCountMessage : AdoCommandMessage
    {
        public long ElapsedMilliseconds { get; protected set; }
        public long? RecordsAffected { get; protected set; }

        public CommandDurationAndRowCountMessage(Guid connectionId, Guid commandId, long elapsedMilliseconds, long? recordsAffected)
            : base(connectionId, commandId)
        {
            CommandId = commandId;
            ElapsedMilliseconds = elapsedMilliseconds;
            RecordsAffected = recordsAffected;
        }
    }
}