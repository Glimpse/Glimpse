using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Mvc3.Interceptor
{
    internal class ModelBinderInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var methodName = method.Name;

            if (methodName.Equals("BindModel")) return interceptors.Where(i => i is BindModelInterceptor).ToArray();
            if (methodName.Equals("BindProperty")) return interceptors.Where(i => i is BindPropertyInterceptor).ToArray();
            if (methodName.Equals("CreateModel")) return interceptors.Where(i => i is CreateModelInterceptor).ToArray();

            return null;
        }
    }
}
