using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core2.Extensibility
{
    public class AlternateImplementationSelector<T> : IInterceptorSelector where T:class
    {
        public IEnumerable<AlternateImplementationToCastleInterceptorAdapter<T>> MethodImplementations { get; set; }

        public AlternateImplementationSelector(IEnumerable<AlternateImplementationToCastleInterceptorAdapter<T>> methodImplementations)
        {
            MethodImplementations = methodImplementations;
        }

        public override bool Equals(object obj)
        {
            var input = obj as AlternateImplementationSelector<T>;

            if (input == null || MethodImplementations.Count() != input.MethodImplementations.Count()) return false;

            return GetHashCode() == input.GetHashCode();//TODO: Better collection comparison
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;

                foreach (var implementation in MethodImplementations)
                {
                    hash = hash * 23 + implementation.GetType().GetHashCode();
                }

                return hash;
            }
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return MethodImplementations.Where(i => i.MethodToImplement == method).ToArray<IInterceptor>();
        }
    }
}