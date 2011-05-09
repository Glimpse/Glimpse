using System;
using System.Reflection;

namespace Glimpse.Net.Warning
{
    internal class NonProxyableMemberWarning:Warning
    {
        public NonProxyableMemberWarning(Type type, MemberInfo memberInfo)
        {
            Message = string.Format("{0} method of {1} type is not proxyable.", memberInfo.Name, type);
            
        }
    }
}
