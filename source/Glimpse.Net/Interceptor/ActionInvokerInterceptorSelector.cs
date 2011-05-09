using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Net.Interceptor
{
    public class ActionInvokerInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var methodName = method.Name;

            if (methodName.Equals("GetFilters")) return interceptors.Where(i => i is GetFiltersInterceptor).ToArray();
            if (methodName.Equals("InvokeActionMethod")) return interceptors.Where(i => i is InvokeActionMethodInterceptor).ToArray();
            if (methodName.Equals("InvokeActionResult")) return interceptors.Where(i => i is InvokeActionResultInterceptor).ToArray();
                
            return null;
        }
    }
}