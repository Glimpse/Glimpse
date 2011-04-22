using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Glimpse.Net.Extensions;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseControllerFactory:IControllerFactory
    {
        public IControllerFactory ControllerFactory { get; set; }

        public GlimpseControllerFactory(IControllerFactory controllerFactory)
        {
            ControllerFactory = controllerFactory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            var iController = ControllerFactory.CreateController(requestContext, controllerName);

            iController.TryWrapActionInvoker();

            return iController;
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            var controllerSessionBehavior = ControllerFactory.GetControllerSessionBehavior(requestContext, controllerName);
            
            return controllerSessionBehavior;
        }

        public void ReleaseController(IController controller)
        {
            ControllerFactory.ReleaseController(controller);
        }
    }
}
