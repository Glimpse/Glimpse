using System.Web.Mvc;

namespace Glimpse.Net.Warning
{
    internal class NotAControllerWarning:Warning
    {
        public NotAControllerWarning(IController controller)
        {
            Message = controller.GetType() + " is not a System.Web.Mvc.Controller.";
        }
    }
}
