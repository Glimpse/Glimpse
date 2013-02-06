using System;
using System.Reflection;

namespace Glimpse.Core.Message
{
    public interface IMessage
    {
        Guid Id { get; }

        Type ExecutedType { get; } // TODO: Does this belong here?

        MethodInfo ExecutedMethod { get; } // TODO: Does this belong here?
    }
}