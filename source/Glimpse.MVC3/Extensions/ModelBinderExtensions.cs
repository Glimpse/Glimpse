using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;
using Glimpse.Mvc3.Interceptor;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ModelBinderExtensions
    {
        internal static bool CanSupportDynamicProxy(this IModelBinder modelBinder, IGlimpseLogger logger)
        {
            if (modelBinder is DefaultModelBinder)
            {
                //Make sure there is a parameterless constructor and the type is not sealed
                var modelBinderType = modelBinder.GetType();
                var proxy = modelBinder as IProxyTargetAccessor;
                var defaultConstructor = modelBinderType.GetConstructor(new Type[] { });

                var result = (!modelBinderType.IsSealed &&
                        defaultConstructor != null &&
                        proxy == null);

                if (!result)
                    logger.Warn("Cannot create proxy of " + modelBinder.GetType() +". Object must have a parameterless constructor, cannot be sealed, and cannot already be a proxy object.");

                return result;
            }

            logger.Warn(modelBinder.GetType() + " is not a System.Web.Mvc.DefaultModelBinder.");
            return false;
        }

        internal static IModelBinder CreateDynamicProxy(this IModelBinder modelBinder, IGlimpseLogger logger)
        {
            var proxyConfig = new Dictionary<string, IInterceptor>
                                  {
                                      {"BindModel", new BindModelInterceptor()},
                                      {"BindProperty", new BindPropertyInterceptor()},
                                      {"CreateModel", new CreateModelInterceptor()},
                                  };
            
            var proxyGenerator = new ProxyGenerator();
            var proxyGenOptions = new ProxyGenerationOptions(new SimpleProxyGenerationHook(logger, proxyConfig.Keys.ToArray())) { Selector = new SimpleInterceptorSelector(proxyConfig) };
            return (IModelBinder)proxyGenerator.CreateClassProxy(modelBinder.GetType(), proxyGenOptions, proxyConfig.Values.ToArray());
        }

        internal static IModelBinder Wrap(this IModelBinder modelBinder)
        {
            return new GlimpseModelBinder(modelBinder);
        }
    }
}
