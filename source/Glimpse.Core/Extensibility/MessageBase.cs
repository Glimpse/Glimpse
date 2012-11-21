using System;
using Glimpse.Core.Message;

namespace Glimpse.Core.Extensibility
{
    public class MessageBase : IMessage
    {
        public MessageBase() : this(Guid.NewGuid())
        {
        }

        public MessageBase(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; } 
    }
}