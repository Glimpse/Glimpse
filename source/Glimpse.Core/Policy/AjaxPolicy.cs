using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Policy which will limit Glimpse to only <c>ModifyResponseHeaders</c> if a Http request is an Ajax request.
    /// </summary>
    public class AjaxPolicy : IRuntimePolicy
    {
        /// <summary>
        /// Gets the point in an Http request lifecycle that a policy should execute.
        /// </summary>
        /// <value>
        /// The moment to execute, <see cref="AjaxPolicy"/> uses <c>BeginRequest</c>.
        /// </value>
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginRequest; }
        }

        /// <summary>
        /// Executes the specified policy with the given context.
        /// </summary>
        /// <param name="policyContext">The policy context.</param>
        /// <returns>
        /// <c>ModifyResponseHeaders</c> if request is an Ajax request, otherwise <c>On</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="policyContext"/> is <c>null</c>.</exception>
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            if (policyContext == null)
            {
                throw new ArgumentNullException("policyContext");
            }

            try
            {
                return policyContext.RequestMetadata.RequestIsAjax ? RuntimePolicy.ModifyResponseHeaders : RuntimePolicy.On;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(Resources.ExecutePolicyWarning, exception, GetType());
                return RuntimePolicy.ModifyResponseHeaders;
            }
        }
    }
}