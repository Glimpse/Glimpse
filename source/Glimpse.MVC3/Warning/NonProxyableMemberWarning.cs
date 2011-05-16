using System;
using System.Reflection;

namespace Glimpse.Mvc3.Warning
{
    internal class NonProxyableMemberWarning:Net.Warning.Warning
    {
        public NonProxyableMemberWarning(Type type, MemberInfo memberInfo)
        {
            Message = string.Format("{0} method of {1} type is not proxyable.", memberInfo.Name, type);
            
        }
    }
}
