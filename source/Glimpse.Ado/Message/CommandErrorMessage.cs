using System;

namespace Glimpse.Ado.Message
{
    public class CommandErrorMessage : AdoCommandMessage
    {
        public CommandErrorMessage(Guid connectionId, Guid commandId, TimeSpan elapsed, Exception exception)
            : base(connectionId, commandId)
        {
            Exception = exception;
            Elapsed = elapsed;
        }

        public Exception Exception { get; protected set; }

        public TimeSpan Elapsed { get; protected set; }
    }
}