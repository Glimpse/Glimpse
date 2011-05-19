using System;
using System.Reflection;

namespace Glimpse.Mvc3.Warning
{
    internal class NonVirtualMemberWarning:WebForms.Warning.Warning
    {
        public NonVirtualMemberWarning(Type type, MemberInfo memberInfo)
        {
            Message = string.Format("{0} method of {1} type is not marked virtual.", memberInfo.Name, type);
        }
    }
}
