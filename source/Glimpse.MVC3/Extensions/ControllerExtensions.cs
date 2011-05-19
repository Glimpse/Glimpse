using System.Web;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Mvc3.Interceptor;
using Glimpse.Mvc3.Warning;
using Glimpse.WebForms.Extensions;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ControllerExtentions
    {
        internal static IController TrySetActionInvoker(this IController iController)
        {
            var warnings = HttpContext.Current.GetWarnings();

            var controller = iController as Controller;
            if (controller == null)
            {
                warnings.Add(new NotAControllerWarning(iController));
                return iController;
            }

            var actionInvoker = controller.ActionInvoker;

            if (actionInvoker.CanSupportDynamicProxy())
            {
                var proxyGenerator = new ProxyGenerator();
                var proxyGenOptions = new ProxyGenerationOptions(new ActionInvokerProxyGenerationHook()) { Selector = new ActionInvokerInterceptorSelector() };
                var newInvoker = (ControllerActionInvoker)proxyGenerator.CreateClassProxy(actionInvoker.GetType(), proxyGenOptions, new InvokeActionMethodInterceptor(), new InvokeActionResultInterceptor(), new GetFiltersInterceptor());
                controller.ActionInvoker = newInvoker;
            }

            return controller;
        }
    }
}
