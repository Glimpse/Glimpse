using System;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public abstract class AdoCommandPayloadMessage<T> : AdoCommandMessage, ITimelineMessage
    {
        protected AdoCommandPayloadMessage(Guid connectionId, Guid commandId, T payload, bool isAsync = false)
            : base(connectionId, commandId)
        {
            Payload = payload;
            IsAsync = isAsync;
        }

        public T Payload { get; protected set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }

        public bool IsAsync { get; set; }
    }
}