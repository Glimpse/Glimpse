using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class AlternateTypeGenerationHook<T> : IProxyGenerationHook
    {
        private IEnumerable<MethodInfo> methodSet;
        
        private IEnumerable<IAlternateMethod> methodImplementations;

        public AlternateTypeGenerationHook(IEnumerable<IAlternateMethod> methodImplementations, ILogger logger)
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

        internal IEnumerable<IAlternateMethod> MethodImplementations
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
            var input = obj as AlternateTypeGenerationHook<T>;

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
            Logger.Debug(Resources.AlternateTypeGenerationHookMethodsInspected, typeof(T));
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Debug(Resources.AlternateTypeGenerationHookNonProxyableMemberNotification, memberInfo.Name, type);
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return MethodImplementations.Any(i => i.MethodToImplement == methodInfo);
        }
    }
}