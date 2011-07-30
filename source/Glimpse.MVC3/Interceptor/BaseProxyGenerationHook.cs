using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Interceptor
{
    internal abstract class BaseProxyGenerationHook:IProxyGenerationHook
    {
        internal IGlimpseLogger Logger { get; set; }

        public BaseProxyGenerationHook(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public abstract IEnumerable<string> GetMethodNames();

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return GetMethodNames().Contains(methodInfo.Name);
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Warn(string.Format("{0} method of {1} type is not proxyable.", memberInfo.Name, type));
        }

        public void MethodsInspected()
        {
        }
    }
}