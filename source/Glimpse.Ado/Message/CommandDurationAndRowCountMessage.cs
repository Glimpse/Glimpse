using System;

namespace Glimpse.Ado.Message
{
    public class CommandDurationAndRowCountMessage : AdoCommandMessage
    {
        public CommandDurationAndRowCountMessage(Guid connectionId, Guid commandId, long? recordsAffected)
            : base(connectionId, commandId)
        {
            CommandId = commandId; 
            RecordsAffected = recordsAffected;
        } 

        public long? RecordsAffected { get; protected set; }
    }
}