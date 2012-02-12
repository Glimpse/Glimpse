using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class AlternateMethodImplementationSelector<T> : IInterceptorSelector where T:class
    {
        public IEnumerable<AlternateMethodImplementationToCastleInterceptorAdapter<T>> MethodImplementations { get; set; }

        public AlternateMethodImplementationSelector(IEnumerable<AlternateMethodImplementationToCastleInterceptorAdapter<T>> methodImplementations)
        {
            MethodImplementations = methodImplementations;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return MethodImplementations.Where(i => i.MethodToImplement == method).ToArray<IInterceptor>();
        }
    }
}