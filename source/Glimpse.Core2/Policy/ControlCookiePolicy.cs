using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
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

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }
    }
}