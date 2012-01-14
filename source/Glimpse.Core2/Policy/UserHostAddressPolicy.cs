using System.Configuration;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    //Commented out to avoid autodiscovery [RuntimePolicy(RuntimeEvent.BeginRequest)]
    public class UserHostAddressPolicy:ConfigurationSection, IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            //TODO: Should we keep this policy, or leverage a simple "isLocal" policy similar to customErrorMode?
            throw new System.NotImplementedException("Have not yet implemented UserHostAddessPolicy");
        }
    }
}