using System;
using System.Web;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Mvc3.Interceptor;
using Glimpse.Mvc3.Plumbing;
using Glimpse.Mvc3.Warning;
using Glimpse.Net.Warning;
using Glimpse.WebForms.Extensions;

namespace Glimpse.Mvc3.Extensions
{
    internal static class ModelBinderExtensions
    {
        internal static bool CanSupportDynamicProxy(this IModelBinder modelBinder)
        {
            var warnings = HttpContext.Current.GetWarnings();

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
                    warnings.Add(new NotProxyableWarning(modelBinder));

                return result;
            }

            warnings.Add(new NotADefaultModelBinderWarning(modelBinder));
            return false;
        }

        internal static IModelBinder CreateDynamicProxy(this IModelBinder modelBinder)
        {
            var proxyGenerator = new ProxyGenerator();
            var proxyGenOptions = new ProxyGenerationOptions(new ModelBinderProxyGenerationHook()) { Selector = new ModelBinderInterceptorSelector() };
            return (IModelBinder)proxyGenerator.CreateClassProxy(modelBinder.GetType(), proxyGenOptions, new BindModelInterceptor(), new BindPropertyInterceptor(), new CreateModelInterceptor());
        }

        internal static IModelBinder Wrap(this IModelBinder modelBinder)
        {
            return new GlimpseModelBinder(modelBinder);
        }
    }
}
