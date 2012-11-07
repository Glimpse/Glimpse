using System;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IActionFilterMessage : IMessage
    {
        bool IsChildAction { get; }

        FilterCategory FilterCategory { get; }

        Type FilterType { get; }

        MethodInfo Method { get; }

        TimeSpan Duration { get; }
    }
}