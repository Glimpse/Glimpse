using System;

namespace Glimpse.Ado.Message
{
    public class CommandErrorMessage : AdoCommandMessage
    {
        public CommandErrorMessage(Guid connectionId, Guid commandId, Exception exception)
            : base(connectionId, commandId)
        {
            Exception = exception; 
        }

        public Exception Exception { get; protected set; } 
    }
}