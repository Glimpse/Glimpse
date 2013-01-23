namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// Definition for a runtime policy that will govern whether a 
    /// the system should run for a particular request. 
    /// </para>
    /// <para>
    /// When implemented, this policy will be added to the pipeline of 
    /// other registered policies. Together these policies determine 
    /// what level of access the system has.
    /// </para>
    /// </summary>
    public interface IRuntimePolicy
    {
        /// <summary>
        /// Gets when the policy should run.
        /// </summary>
        /// <value>The level value.</value>
        RuntimeEvent ExecuteOn { get; }

        /// <summary>
        /// Executes the specified policy in a given context.
        /// </summary>
        /// <param name="policyContext">The policy context.</param>
        /// <returns>What the request is allowed to do.</returns>
        RuntimePolicy Execute(IRuntimePolicyContext policyContext);
    }
}