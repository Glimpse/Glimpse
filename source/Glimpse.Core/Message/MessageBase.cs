using System;
using System.Reflection;

namespace Glimpse.Core.Message
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

        public Guid Id { get; protected internal set; }
    }
}