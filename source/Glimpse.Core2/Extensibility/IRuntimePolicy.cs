namespace Glimpse.Core2.Extensibility
{
    public interface IRuntimePolicy
    {
        RuntimePolicy Execute(IRuntimePolicyContext policyContext);
        RuntimeEvent ExecuteOn { get; }
    }
}