using System;

namespace Glimpse.Core.Extensibility
{
    public class MessageBase
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