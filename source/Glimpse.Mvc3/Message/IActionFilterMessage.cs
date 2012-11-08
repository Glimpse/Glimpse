using System;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IActionFilterMessage : IMessage
    {
        bool IsChildAction { get; }

        FilterCategory? Category { get; }

        Type ExecutedType { get; }

        MethodInfo ExecutedMethod { get; }

        TimeSpan Duration { get; }
    }
}