using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.BeginRequest)]
    public class ControlCookiePolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            var cookie = policyContext.RequestMetadata.GetCookie(Constants.ControlCookieName);

            if (string.IsNullOrEmpty(cookie))
                return RuntimePolicy.Off;
                
            RuntimePolicy result;

            if (!RuntimePolicy.TryParse(cookie, true, out result)) 
                return RuntimePolicy.Off;

            return result;
        }
    }
}