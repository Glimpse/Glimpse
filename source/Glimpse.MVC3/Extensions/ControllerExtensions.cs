using System.Collections.Generic;
using System.Linq;
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
                var proxyConfig = new Dictionary<string, IInterceptor>
                                      {
                                          {"GetFilters", new GetFiltersInterceptor()},
                                          {"InvokeActionResult", new InvokeActionResultInterceptor()},
                                          {"InvokeActionMethod",new InvokeActionMethodInterceptor()}
                                      };

                var proxyGenerator = new ProxyGenerator();
                var proxyGenOptions = new ProxyGenerationOptions(new SimpleProxyGenerationHook(logger, proxyConfig.Keys.ToArray())) { Selector = new SimpleInterceptorSelector(proxyConfig) };
                var newInvoker = (ControllerActionInvoker)proxyGenerator.CreateClassProxy(actionInvoker.GetType(), proxyGenOptions, proxyConfig.Values.ToArray());
                controller.ActionInvoker = newInvoker;
            }

            return controller;
        }
    }
}
