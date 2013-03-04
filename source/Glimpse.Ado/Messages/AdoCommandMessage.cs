using System;

namespace Glimpse.Ado.Messages
{
    public abstract class AdoCommandMessage : AdoMessage
    {
        public Guid CommandId { get; protected set; }

        protected AdoCommandMessage(Guid connectionId, Guid commandId) : base(connectionId)
        {
            CommandId = commandId;
        }
    }
}