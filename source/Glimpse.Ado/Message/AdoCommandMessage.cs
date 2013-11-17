using System;

namespace Glimpse.Ado.Message
{
    public abstract class AdoCommandMessage : AdoMessage
    {
        protected AdoCommandMessage(Guid connectionId, Guid commandId) : base(connectionId)
        {
            CommandId = commandId;
        }

        public Guid CommandId { get; protected set; }
    }
}