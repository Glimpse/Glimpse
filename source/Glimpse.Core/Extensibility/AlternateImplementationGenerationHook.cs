using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class AlternateImplementationGenerationHook<T> : IProxyGenerationHook
    {
        private IEnumerable<MethodInfo> methodSet;
        
        private IEnumerable<IAlternateImplementation> methodImplementations;

        public AlternateImplementationGenerationHook(IEnumerable<IAlternateImplementation> methodImplementations, ILogger logger)
        {
            if (methodImplementations == null)
            {
                throw new ArgumentNullException("methodImplementations");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            
            MethodImplementations = methodImplementations;
            Logger = logger;
        }

        internal IEnumerable<IAlternateImplementation> MethodImplementations
        {
            get
            {
                return methodImplementations;
            }

            set
            {
                methodImplementations = value;
                methodSet = value.Select(m => m.MethodToImplement);
            }
        }

        internal ILogger Logger { get; set; }

        public override bool Equals(object obj)
        {
            var input = obj as AlternateImplementationGenerationHook<T>;

            if (input == null)
            {
                return false;
            }

            var result = methodSet.SequenceEqual(input.MethodImplementations.Select(m => m.MethodToImplement));

            return result;
        }

        public override int GetHashCode()
        {
            // Overflow is fine, just wrap
            unchecked 
            {
                int hash = 17;

                foreach (var implementation in MethodImplementations)
                {
                    hash = (hash * 23) + implementation.GetType().GetHashCode();
                }

                return hash;
            }
        }

        public void MethodsInspected()
        {
            Logger.Debug(Resources.AlternateImplementationGenerationHookMethodsInspected, typeof(T));
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Debug(Resources.AlternateImplementationGenerationHookNonProxyableMemberNotification, memberInfo.Name, type);
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return MethodImplementations.Any(i => i.MethodToImplement == methodInfo);
        }
    }
}