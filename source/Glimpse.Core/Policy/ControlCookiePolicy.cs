using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Policy which will set Glimpse's runtime policy to <c>Off</c> unless a marker cookie named <c>'glimpsePolicy'</c> is present on the Http request.
    /// </summary>
    public class ControlCookiePolicy : IRuntimePolicy
    {
        internal const string ControlCookieName = "glimpsePolicy";

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
        /// <c>Off</c> unless the request contains a 'glimpsePolicy' cookie, otherwise the parsed <see cref="RuntimePolicy"/> of the cookie.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="policyContext"/> is <c>null</c>.</exception>
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            var cookie = policyContext.RequestMetadata.GetCookie(ControlCookieName);

            if (string.IsNullOrEmpty(cookie))
            {
                return RuntimePolicy.Off;
            }
                
            RuntimePolicy result;

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseEnum(cookie, true, out result))
            {
                return RuntimePolicy.Off;
            }
#else
            if (!Enum.TryParse(cookie, true, out result))
            {
                return RuntimePolicy.Off;
            }
#endif

            return result;
        }
    }
}