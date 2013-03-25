using System;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public class CommandErrorMessage : AdoCommandMessage, ITimelineMessage
    {
        public CommandErrorMessage(Guid connectionId, Guid commandId, Exception exception)
            : base(connectionId, commandId)
        {
            Exception = exception; 
        }

        public Exception Exception { get; protected set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }
    }
}