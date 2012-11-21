using System.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    public class UserHostAddressPolicy : ConfigurationSection, IRuntimePolicy
    {
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            // TODO: Should we keep this policy, or leverage a simple "isLocal" policy similar to customErrorMode?
            return RuntimePolicy.On;
        }
    }
}