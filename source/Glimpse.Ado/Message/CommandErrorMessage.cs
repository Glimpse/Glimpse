using System;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public class CommandErrorMessage : AdoCommandMessage, ITimelineMessage
    {
        public CommandErrorMessage(Guid connectionId, Guid commandId, Exception exception)
            : this(connectionId, commandId, exception, false)
        { }

        public CommandErrorMessage(Guid connectionId, Guid commandId, Exception exception, bool isAsync)
            : base(connectionId, commandId)
        {
            Exception = exception;
            IsAsync = isAsync;
        }

        public Exception Exception { get; protected set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }

        public bool IsAsync { get; set; }
    }
}