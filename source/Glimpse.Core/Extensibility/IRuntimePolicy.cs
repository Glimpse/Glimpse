namespace Glimpse.Core.Extensibility
{
    public interface IRuntimePolicy
    {
        RuntimeEvent ExecuteOn { get; }
        
        RuntimePolicy Execute(IRuntimePolicyContext policyContext);
    }
}