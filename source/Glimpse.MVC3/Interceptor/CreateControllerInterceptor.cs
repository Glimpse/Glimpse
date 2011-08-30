using System.Diagnostics;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Interceptor
{
    internal class CreateControllerInterceptor:IInterceptor
    {
        internal IGlimpseLogger Logger { get; set; }

        public CreateControllerInterceptor(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            //Intercepting: CreateController(RequestContext requestContext, string controllerName)

            var controllerName = invocation.Arguments[1];

            invocation.Proceed();

            var controller = invocation.ReturnValue as IController;

            //IController controller = ControllerFactory.CreateController(requestContext, controllerName);

            Trace.Write(string.Format("{0}.CreateController(requestContext, \"{1}\") = {2}", base.GetType().Name, controllerName, controller == null ? "null" : controller.GetType().ToString()));

            if (controller != null)
            {
                invocation.ReturnValue = controller.TrySetActionInvoker(Logger);
                return;
            }

            Logger.Warn("Unable to proxy controller named {0}", controllerName);
            invocation.ReturnValue = null;

        }
    }
}
