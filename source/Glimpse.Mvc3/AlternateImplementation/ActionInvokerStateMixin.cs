using System;

namespace Glimpse.Mvc.AlternateImplementation
{
    public interface IActionInvokerStateMixin
    {
        TimeSpan Offset { get; set; }

        ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }

    public class ActionInvokerStateMixin : IActionInvokerStateMixin
    {
        public TimeSpan Offset { get; set; }

        public ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }
}
