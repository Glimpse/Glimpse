using System;
using System.Reflection;
using System.Web;
using Castle.DynamicProxy;
using Glimpse.Net.Extensions;
using Glimpse.Net.Warning;

namespace Glimpse.Net.Interceptor
{
    public class ProxyGenerationHook : IProxyGenerationHook
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            var methodName = methodInfo.Name;

            var result =  (methodName.Equals("GetFilters") ||
                    methodName.Equals("InvokeActionResult") ||
                    methodName.Equals("InvokeActionMethod"));

            return result;
        }

        public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
        {
            var warnings = HttpContext.Current.GetWarnings();
            warnings.Add(new NonVirtualMemberWarning(type, memberInfo));
        }

        public void MethodsInspected()
        {
        }
    }
}