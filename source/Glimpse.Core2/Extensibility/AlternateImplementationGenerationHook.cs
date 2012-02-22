using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core2.Extensibility
{
    public class AlternateImplementationGenerationHook<T> : IProxyGenerationHook where T:class
    {
        public IEnumerable<IAlternateImplementation<T>> MethodImplementations { get; set; }
        public ILogger Logger { get; set; }

        public AlternateImplementationGenerationHook(IEnumerable<IAlternateImplementation<T>> methodImplementations, ILogger logger)
        {
            Contract.Requires<ArgumentNullException>(methodImplementations != null, "methodImplementations");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");

            MethodImplementations = methodImplementations;
            Logger = logger;
        }

        public override bool Equals(object obj)
        {
            var input = obj as AlternateImplementationGenerationHook<T>;

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

        public void MethodsInspected()
        {
            Logger.Debug("Methods inspected on type '{0}'.", typeof(T));//TODO: Add to resx file
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Debug("{0} method of {1} type is not proxyable.", memberInfo.Name, type);//TODO: Add to resx file
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return MethodImplementations.Any(i => i.MethodToImplement == methodInfo);
        }
    }
}