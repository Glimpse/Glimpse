using System.Configuration;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    public class UserHostAddressPolicy:ConfigurationSection, IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            //TODO: Should we keep this policy, or leverage a simple "isLocal" policy similar to customErrorMode?
            return RuntimePolicy.On;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }
    }
}