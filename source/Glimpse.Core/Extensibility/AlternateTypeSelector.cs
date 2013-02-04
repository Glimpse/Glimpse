using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class AlternateTypeSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return interceptors.Where(i =>
                                          {
                                              var ai = i as AlternateTypeToCastleInterceptorAdapter;
                                              return ai != null && ai.MethodToImplement == method;
                                          }).ToArray();
        }
    }
}