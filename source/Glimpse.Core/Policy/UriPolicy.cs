using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Policy which will set Glimpse's runtime policy to <c>Off</c> if a Http request's Uri matches a configured pattern
    /// </summary>
    public class UriPolicy : IRuntimePolicy, IConfigurableExtended
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriPolicy" />
        /// </summary>
        public UriPolicy()
        {
            Configurator = new UriPolicyConfigurator();
        }

        /// <summary>
        /// Gets the <see cref="UriPolicyConfigurator" /> used by the <see cref="UriPolicy" />
        /// </summary>
        public UriPolicyConfigurator Configurator { get; private set; }

        /// <summary>
        /// Gets the configurator
        /// </summary>
        IConfigurator IConfigurableExtended.Configurator { get { return Configurator; } }

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
        /// <c>On</c> if the request Uri does not match a configured uri pattern, otherwise <c>Off</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="policyContext"/> is <c>null</c>.</exception>
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                if (!Configurator.ContainsUriPatternsToIgnore)
                {
                    return RuntimePolicy.On;
                }

                var uri = policyContext.RequestMetadata.RequestUri.AbsoluteUri;

                return Configurator.UriPatternsToIgnore.Any(regex => regex.IsMatch(uri)) ? RuntimePolicy.Off : RuntimePolicy.On;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(Resources.ExecutePolicyWarning, exception, GetType());
                return RuntimePolicy.Off;
            }
        }
    }
}