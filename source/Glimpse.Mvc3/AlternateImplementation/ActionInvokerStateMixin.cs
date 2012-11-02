namespace Glimpse.Mvc.AlternateImplementation
{
    public interface IActionInvokerStateMixin
    {
        long Offset { get; set; }

        ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }

    public class ActionInvokerStateMixin : IActionInvokerStateMixin
    {
        public long Offset { get; set; }

        public ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }
}
