using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    public class GlimpseResourcePolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            return RuntimePolicy.ExecuteResourceOnly; //Don't run Glimpse methods except to execute resource.
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.ExecuteResource; }
        }
    }
}