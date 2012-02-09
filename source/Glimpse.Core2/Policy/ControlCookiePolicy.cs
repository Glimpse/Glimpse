using System;
using Glimpse.Core2.Extensibility;

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

            if (!Enum.TryParse(cookie, true, out result)) 
                return RuntimePolicy.Off;

            return result;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }
    }
}