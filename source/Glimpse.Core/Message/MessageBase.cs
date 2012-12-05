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

        public MessageBase(Type executedType, MethodInfo executedMethod) : this()
        {
            ExecutedType = executedType;
            ExecutedMethod = executedMethod;
        }

        public Guid Id { get; protected internal set; }

        public Type ExecutedType { get; private set; }

        public MethodInfo ExecutedMethod { get; private set; }
    }
}