using System;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IActionBaseMessage : ITimelineMessage
    {
        bool IsChildAction { get; }
    }
}