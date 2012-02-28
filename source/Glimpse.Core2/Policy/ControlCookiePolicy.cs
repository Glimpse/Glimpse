using System;
using Glimpse.Core2.Extensibility;
#if NET35
using Glimpse.Core2.Backport;
#endif

namespace Glimpse.Core2.Policy
{
    public class ControlCookiePolicy:IRuntimePolicy
    {
        internal const string ControlCookieName = "glimpsePolicy";

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            var cookie = policyContext.RequestMetadata.GetCookie(ControlCookieName);

            if (String.IsNullOrEmpty(cookie))
                return RuntimePolicy.Off;
                
            RuntimePolicy result;

#if NET35
            if (!Net35Backport.TryParseEnum(cookie, true, out result))
                return RuntimePolicy.Off;
#else
            if (!Enum.TryParse(cookie, true, out result)) 
                return RuntimePolicy.Off;
#endif

            return result;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }
    }
}