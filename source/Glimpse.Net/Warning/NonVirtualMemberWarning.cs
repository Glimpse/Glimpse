using System;
using System.Reflection;

namespace Glimpse.Net.Warning
{
    internal class NonVirtualMemberWarning:Warning
    {
        public NonVirtualMemberWarning(Type type, MemberInfo memberInfo)
        {
            Message = string.Format("{0} method of {1} type is not marked virtual.", memberInfo.Name, type);
        }
    }
}
