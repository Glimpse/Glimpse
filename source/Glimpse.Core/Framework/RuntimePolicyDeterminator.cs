using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
#if NET35
using Glimpse.Core.Backport;
#endif

namespace Glimpse.Core.Framework
{
    internal class RuntimePolicyDeterminator
    {
        private IReadonlyConfiguration Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimePolicyDeterminator" />
        /// </summary>
        /// <param name="configuration">The <see cref="IReadonlyConfiguration"/> that should be used</param>
        public RuntimePolicyDeterminator(IReadonlyConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Determines the resulting <see cref="RuntimePolicy"/> based on the available <see cref="IRuntimePolicy"/>
        /// </summary>
        /// <param name="runtimeEvent">The <see cref="RuntimeEvent"/></param>
        /// <param name="maximumAllowedPolicy">The <see cref="RuntimePolicy"/> that start from, this is the highest possible <see cref="RuntimePolicy"/> that can be returned</param>
        /// <param name="requestResponseAdapter">The <see cref="IRequestResponseAdapter"/></param>
        /// <returns>The <see cref="RuntimePolicy"/> that is currently in effect</returns>
        public RuntimePolicy DetermineRuntimePolicy(RuntimeEvent runtimeEvent, RuntimePolicy maximumAllowedPolicy, IRequestResponseAdapter requestResponseAdapter)
        {
            if (maximumAllowedPolicy == RuntimePolicy.Off)
            {
                return maximumAllowedPolicy;
            }

            var logger = Configuration.Logger;

            var policies = Configuration.RuntimePolicies.Where(policy => policy.ExecuteOn.HasFlag(runtimeEvent));
            var policyContext = new RuntimePolicyContext(requestResponseAdapter.RequestMetadata, logger, requestResponseAdapter.RuntimeContext);
            foreach (var policy in policies)
            {
                var policyResult = RuntimePolicy.Off;
                try
                {
                    policyResult = policy.Execute(policyContext);

                    if (policyResult != RuntimePolicy.On)
                    {
                        logger.Debug("RuntimePolicy '{0}' has been returned by IRuntimePolicy of type '{1}' during RuntimeEvent '{2}'.", policyResult, policy.GetType(), runtimeEvent);
                    }
                }
                catch (Exception exception)
                {
                    logger.Warn("Exception thrown when executing IRuntimePolicy of type '{0}'. The resulting RuntimePolicy will be set to 'Off'. {1}Exception: {2}", policy.GetType(), Environment.NewLine, exception);
                }

                // Only use the lowest policy allowed for the request
                if (policyResult < maximumAllowedPolicy)
                {
                    maximumAllowedPolicy = policyResult;
                }

                // If the policy indicates Glimpse is Off, then we stop processing any other runtime policy
                if (maximumAllowedPolicy == RuntimePolicy.Off)
                {
                    break;
                }
            }

            return maximumAllowedPolicy;
        }
    }
}