using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    public class ControlCookiePolicy : IRuntimePolicy
    {
        internal const string ControlCookieName = "glimpsePolicy";

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            var cookie = policyContext.RequestMetadata.GetCookie(ControlCookieName);

            if (string.IsNullOrEmpty(cookie))
            {
                return RuntimePolicy.Off;
            }
                
            RuntimePolicy result;

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseEnum(cookie, true, out result))
            {
                return RuntimePolicy.Off;
            }
#else
            if (!Enum.TryParse(cookie, true, out result))
            {
                return RuntimePolicy.Off;
            }
#endif

            return result;
        }
    }
}