namespace Glimpse.Mvc.AlternateImplementation
{
    public interface IActionInvokerState
    {
        long Offset { get; set; }

        ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }

    public class ActionInvokerState : IActionInvokerState
    {
        public long Offset { get; set; }

        public ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }
}
