using System;
using System.Diagnostics;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Message
{
    public abstract class AdoMessage : IMessage
    {
        protected AdoMessage(Guid connectionId)
        {
            Id = Guid.NewGuid();
            ConnectionId = connectionId;
            StartTime = DateTime.Now;            
        }

        public Guid Id { get; private set; }
        public Guid ConnectionId { get; set; }    
        public DateTime StartTime { get; set; }
    }
}