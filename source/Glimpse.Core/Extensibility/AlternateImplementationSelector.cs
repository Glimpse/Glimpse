using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class AlternateImplementationSelector<T> : IInterceptorSelector where T:class
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return interceptors.Where(i =>
                                          {
                                              var ai = i as AlternateImplementationToCastleInterceptorAdapter<T>;
                                              return (ai != null && ai.MethodToImplement == method);
                                          }).ToArray();
        }
    }
}