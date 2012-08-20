using System.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core;

namespace Glimpse.Core.Policy
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