using System;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Ado.Messages
{
    public class AdoMessage : IMessage
    {
        public Guid Id { get; private set; }
        public Type ExecutedType { get; private set; }
        public MethodInfo ExecutedMethod { get; private set; }
    }

    public class FooMessage : AdoMessage
    {
        
    }

    public class BarMessage : AdoMessage
    {
        
    }
}