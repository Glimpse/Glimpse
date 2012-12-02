using System;

namespace Glimpse.Core.Message
{
    public class MessageBase : IMessage
    {
        public MessageBase() : this(Guid.NewGuid())
        {
        }

        public MessageBase(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; } 
    }
}