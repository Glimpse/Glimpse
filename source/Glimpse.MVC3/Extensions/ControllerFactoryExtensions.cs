using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Interceptor;
using Castle.DynamicProxy;
using System;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ControllerFactoryExtentions
    {
        internal static IControllerFactory Proxy(this IControllerFactory iControllerFactory, IGlimpseLogger logger)
        {

            if (iControllerFactory.CanSupportDynamicProxy(logger))
            {
                var proxyConfig = new Dictionary<string, IInterceptor>
                                      {
                                          {"CreateController", new CreateControllerInterceptor(logger)}
                                      };

                var proxyGenerator = new ProxyGenerator();
                var proxyGenOptions = new ProxyGenerationOptions(new SimpleProxyGenerationHook(logger, proxyConfig.Keys.ToArray())) { Selector = new SimpleInterceptorSelector(proxyConfig) };
                var newInvoker = (IControllerFactory)proxyGenerator.CreateClassProxy(iControllerFactory.GetType(), proxyGenOptions, proxyConfig.Values.ToArray());

                return newInvoker;
            }

            logger.Warn("Unable to proxy {0}", iControllerFactory.GetType());

            return iControllerFactory;



            //if (iControllerFactory is GlimpseControllerFactory) return iControllerFactory;

            //return new GlimpseControllerFactory(iControllerFactory, logger);
        }

        internal static bool CanSupportDynamicProxy(this IControllerFactory controllerFactory, IGlimpseLogger logger)
        {
                //Make sure there is a parameterless constructor and the type is not sealed
                var controllerFactoryType = controllerFactory.GetType();
                var proxy = controllerFactory as IProxyTargetAccessor;
                var defaultConstructor = controllerFactoryType.GetConstructor(new Type[] { });

                var result = (!controllerFactoryType.IsSealed &&
                        defaultConstructor != null &&
                        proxy == null);

                if (!result)
                {
                    //TODO: Add logging
                }

                return result;
        }
    }
}