using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

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

            //controller does not exist
            if (iController == null) return iController;

            var controller = iController as Controller;

            //custom controller
            if (controller == null) return iController;

            //already swapped out the invoker
            if (controller.ActionInvoker is GlimpseActionInvoker) return iController;

            //swap out action invoker
            if (controller.ActionInvoker is ControllerActionInvoker) controller.ActionInvoker = new GlimpseActionInvoker();
            return controller;
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
