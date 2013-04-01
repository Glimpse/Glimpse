using System;
using System.Diagnostics;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public abstract class AdoMessage : ITimedMessage
    {
        protected AdoMessage(Guid connectionId)
        {
            Id = Guid.NewGuid();
            ConnectionId = connectionId;
            StartTime = DateTime.Now;            
        }

        public Guid Id { get; private set; }

        public Guid ConnectionId { get; set; }

        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }
    }
}