using System;
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
            var proxyGenerator = new ProxyGenerator();
            var proxyGenOptions = new ProxyGenerationOptions(new SimpleProxyGenerationHook(logger, "BindModel", "BindProperty", "CreateModel")) { Selector = new ModelBinderInterceptorSelector() };
            return (IModelBinder)proxyGenerator.CreateClassProxy(modelBinder.GetType(), proxyGenOptions, new BindModelInterceptor(), new BindPropertyInterceptor(), new CreateModelInterceptor());
        }

        internal static IModelBinder Wrap(this IModelBinder modelBinder)
        {
            return new GlimpseModelBinder(modelBinder);
        }
    }
}
