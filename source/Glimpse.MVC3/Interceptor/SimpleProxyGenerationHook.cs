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

        /*public bool Equals(SimpleProxyGenerationHook other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result =  Equals(other.Logger, Logger) && Equals(other.Methods, Methods);
            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SimpleProxyGenerationHook)) return false;
            var result = Equals((SimpleProxyGenerationHook) obj);
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = ((Logger != null ? Logger.GetHashCode() : 0)*397) ^ (Methods != null ? Methods.GetHashCode() : 0);
                return result;
            }
        }*/
    }
}