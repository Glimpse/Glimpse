using System;

namespace Glimpse.Ado.Message
{
    public class CommandErrorMessage : AdoCommandMessage
    {
        public Exception Exception { get; protected set; }

        public CommandErrorMessage(Guid connectionId, Guid commandId, Exception exception) : base(connectionId, commandId)
        {
            Exception = exception;
        }
    }
}