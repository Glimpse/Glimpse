using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Interceptor
{
    internal class SimpleProxyGenerationHook:IProxyGenerationHook
    {
        internal IGlimpseLogger Logger { get; set; }
        internal string[] Methods { get; set; }

        public SimpleProxyGenerationHook(IGlimpseLogger logger, string[] methods)
        {
            Logger = logger;
            Methods = methods;
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return Methods.Contains(methodInfo.Name);
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