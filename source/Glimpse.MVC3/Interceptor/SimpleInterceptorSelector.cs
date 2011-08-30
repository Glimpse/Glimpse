using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Mvc3.Interceptor
{
    internal class SimpleInterceptorSelector:IInterceptorSelector
    {
        internal IDictionary<string, IInterceptor> Interceptors { get; set; }

        public SimpleInterceptorSelector(IDictionary<string, IInterceptor> interceptors)
        {
            Interceptors = interceptors;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return (from i in Interceptors where i.Key == method.Name select i.Value).ToArray();
        }
    }
}