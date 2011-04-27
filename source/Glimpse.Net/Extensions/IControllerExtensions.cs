using System;
using System.Web;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Net.Interceptor;
using Glimpse.Net.Plumbing;
using Castle.Core.Interceptor;
using Glimpse.Net.Warning;

namespace Glimpse.Net.Extensions
{
    public static class IControllerExtentions
    {
        public static IController TrySetActionInvoker(this IController iController)
        {
            var warnings = HttpContext.Current.GetWarnings();

            var controller = iController as Controller;
            if (controller == null)
            {
                warnings.Add(new NotAControllerWarning(iController));
                return iController;
            }

            var actionInvoker = controller.ActionInvoker;

            if (CanSupportDynamicProxy(actionInvoker))
            {
                var proxyGenerator = new ProxyGenerator();
                var proxyGenOptions = new ProxyGenerationOptions(new ProxyGenerationHook()) { Selector = new ActionInvokerInterceptorSelector() };
                var newInvoker = (ControllerActionInvoker)proxyGenerator.CreateClassProxy(actionInvoker.GetType(), proxyGenOptions, new InvokeActionMethodInterceptor(), new InvokeActionResultInterceptor(), new GetFiltersInterceptor());
                controller.ActionInvoker = newInvoker;
            }

            return controller;
        }

        private static bool CanSupportDynamicProxy(IActionInvoker actionInvoker)
        {
            var warnings = HttpContext.Current.GetWarnings();

            if (actionInvoker is ControllerActionInvoker)//TODO: What changes for AsyncControllerActionInvoker?
            {
                //Make sure there is a parameterless constructor and the type is not sealed
                var actionInvokerType = actionInvoker.GetType();
                var proxy = actionInvoker as IProxyTargetAccessor;
                var defaultConstructor = actionInvokerType.GetConstructor(new Type[] { });

                var result = (!actionInvokerType.IsSealed &&
                        defaultConstructor != null &&
                        proxy == null);

                if (!result)
                    warnings.Add(new NotProxyableWarning(actionInvoker));

                return result;
            }

            warnings.Add(new NotAControllerActionInvokerWarning(actionInvoker));
            return false;
        }
    }
}
