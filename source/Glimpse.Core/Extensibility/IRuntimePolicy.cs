namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// <c>IRuntimePolicy</c> controls the operations Glimpse is allowed to do an Http request.
    /// </para>
    /// <para>
    /// When implemented, a policy will be discovered and added to the collection of policies. 
    /// Together, all policies determine what Glimpse is allowed to do during an Http request.
    /// </para>
    /// </summary>
    public interface IRuntimePolicy
    {
        /// <summary>
        /// Gets the point in an Http request lifecycle that a policy should execute.
        /// </summary>
        /// <value>The moment to execute.</value>
        RuntimeEvent ExecuteOn { get; }

        /// <summary>
        /// Executes the specified policy with the given context.
        /// </summary>
        /// <param name="policyContext">The policy context.</param>
        /// <returns>A value describing what Glimpse is allowed to do during the request.</returns>
        RuntimePolicy Execute(IRuntimePolicyContext policyContext);
    }
}