using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Policy which will limit Glimpse to only <c>ExecuteResourceOnly</c> if a Http request for an implementation of <see cref="IResource"/>.
    /// </summary>
    public class GlimpseResourcePolicy : IRuntimePolicy
    {
        /// <summary>
        /// Gets the point in an Http request lifecycle that a policy should execute.
        /// </summary>
        /// <value>
        /// The moment to execute, <see cref="AjaxPolicy"/> uses <c>ExecuteResource</c>.
        /// </value>
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.ExecuteResource; }
        }

        /// <summary>
        /// Executes the specified policy with the given context.
        /// </summary>
        /// <param name="policyContext">The policy context.</param>
        /// <returns>
        /// <c>ExecuteResourceOnly</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="policyContext"/> is <c>null</c>.</exception>
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            return RuntimePolicy.ExecuteResourceOnly; // Don't run Glimpse methods except to execute resource.
        }
    }
}