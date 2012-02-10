using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Policy
{
    public class GlimpseResourcePolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            return RuntimePolicy.Ignore; //Allow Glimpse to run, but don't change the response at all
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.ExecuteResource; }
        }
    }
}