using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.ExecuteResource)]
    public class GlimpseResourcePolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            return RuntimePolicy.Off; //Never run Glimpse on requests for its own resources
        }
    }
}