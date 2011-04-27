using System.Web.Mvc;

namespace Glimpse.Net.Warning
{
    internal class NotAControllerActionInvokerWarning:Warning
    {
        public NotAControllerActionInvokerWarning(IActionInvoker actionInvoker)
        {
            Message = actionInvoker.GetType() + " is not a System.Web.Mvc.ControllerActionInvoker.";
        }
    }
}
