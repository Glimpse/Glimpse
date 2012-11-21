namespace Glimpse.Mvc.AlternateImplementation
{
    public interface IActionInvokerStateMixin
    {
        double Offset { get; set; }

        ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }

    public class ActionInvokerStateMixin : IActionInvokerStateMixin
    {
        public double Offset { get; set; }

        public ActionInvoker.InvokeActionMethod.Arguments Arguments { get; set; }
    }
}
