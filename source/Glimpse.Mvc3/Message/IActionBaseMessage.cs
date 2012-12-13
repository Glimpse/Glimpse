using System;
using System.Reflection;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IActionBaseMessage
    {
        bool IsChildAction { get; }
    }
}