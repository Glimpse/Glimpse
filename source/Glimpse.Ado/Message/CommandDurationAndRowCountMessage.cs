using System;

namespace Glimpse.Ado.Message
{
    public class CommandDurationAndRowCountMessage : AdoCommandMessage
    {
        public CommandDurationAndRowCountMessage(Guid connectionId, Guid commandId, TimeSpan elapsed, long? recordsAffected)
            : base(connectionId, commandId)
        {
            CommandId = commandId;
            Elapsed = elapsed;
            RecordsAffected = recordsAffected;
        }

        public TimeSpan Elapsed { get; protected set; }

        public long? RecordsAffected { get; protected set; }
    }
}