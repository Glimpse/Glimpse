using System;
using System.Reflection;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Interceptor
{
    internal class ActionInvokerProxyGenerationHook : IProxyGenerationHook
    {
        internal IGlimpseLogger Logger { get; set; }

        public ActionInvokerProxyGenerationHook(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            var methodName = methodInfo.Name;

            var result =  (methodName.Equals("GetFilters") ||
                    methodName.Equals("InvokeActionResult") ||
                    methodName.Equals("InvokeActionMethod"));

            return result;
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Warn(string.Format("{0} method of {1} type is not proxyable.", memberInfo.Name, type));
        }

        public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Warn(string.Format("{0} method of {1} type is not marked virtual.", memberInfo.Name, type));
        }

        public void MethodsInspected()
        {
        }
    }
}