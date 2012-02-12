using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class AlternateMethodImplementationGenerationHook<T> : IProxyGenerationHook where T:class
    {
        public IEnumerable<IAlternateMethodImplementation<T>> MethodImplementations { get; set; }
        public ILogger Logger { get; set; }

        public AlternateMethodImplementationGenerationHook(IEnumerable<IAlternateMethodImplementation<T>> methodImplementations, ILogger logger)
        {
            MethodImplementations = methodImplementations;
            Logger = logger;
        }

        public void MethodsInspected()
        {
            Logger.Debug("Methods inspected on type '{0}'.", typeof(T));
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Debug("{0} method of {1} type is not proxyable.", memberInfo.Name, type);
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return MethodImplementations.Any(i => i.MethodToImplement == methodInfo);
        }
    }
}