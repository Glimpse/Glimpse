using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Interceptor;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ControllerExtentions
    {
        internal static IController TrySetActionInvoker(this IController iController, IGlimpseLogger logger)
        {
            var controller = iController as Controller;
            if (controller == null)
            {
                //TODO: Add Logging
                return iController;
            }

            var actionInvoker = controller.ActionInvoker;

            if (actionInvoker.CanSupportDynamicProxy(logger))
            {
                var proxyGenerator = new ProxyGenerator();
                var proxyGenOptions = new ProxyGenerationOptions(new SimpleProxyGenerationHook(logger, "GetFilters", "InvokeActionResult", "InvokeActionMethod")) { Selector = new ActionInvokerInterceptorSelector() };
                var newInvoker = (ControllerActionInvoker)proxyGenerator.CreateClassProxy(actionInvoker.GetType(), proxyGenOptions, new InvokeActionMethodInterceptor(), new InvokeActionResultInterceptor(), new GetFiltersInterceptor());
                controller.ActionInvoker = newInvoker;
            }

            return controller;
        }
    }
}
