using Glimpse.Core;

namespace Glimpse.Core.Extensibility
{
    public interface IRuntimePolicy
    {
        RuntimePolicy Execute(IRuntimePolicyContext policyContext);
        RuntimeEvent ExecuteOn { get; }
    }
}