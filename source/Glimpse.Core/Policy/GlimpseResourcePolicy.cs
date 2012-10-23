using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    public class GlimpseResourcePolicy : IRuntimePolicy
    {
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.ExecuteResource; }
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            return RuntimePolicy.ExecuteResourceOnly; // Don't run Glimpse methods except to execute resource.
        }
    }
}