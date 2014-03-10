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
        private IRuntimePolicy[] AvailableRuntimePolicies { get; set; }
        private ILogger Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimePolicyDeterminator" />
        /// </summary>
        /// <param name="availableRuntimePolicies">The <see cref="IRuntimePolicy"/> that will be used to determine a resulting <see cref="RuntimePolicy"/></param>
        /// <param name="logger">The <see cref="ILogger"/> that will be used when executing <see cref="IRuntimePolicy"/> instances</param>
        public RuntimePolicyDeterminator(IRuntimePolicy[] availableRuntimePolicies, ILogger logger)
        {
            if (availableRuntimePolicies == null)
            {
                throw new ArgumentNullException("availableRuntimePolicies");
            }

            AvailableRuntimePolicies = availableRuntimePolicies;

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Logger = logger;
        }

        /// <summary>
        /// Determines the resulting <see cref="RuntimePolicy"/> based on the available <see cref="IRuntimePolicy"/>
        /// </summary>
        /// <param name="runtimeEvent">The <see cref="RuntimeEvent"/></param>
        /// <param name="maximumAllowedPolicy">The <see cref="RuntimePolicy"/> that start from, this is the highest possible <see cref="RuntimePolicy"/> that can be returned</param>
        /// <param name="requestResponseAdapter">The <see cref="IRequestResponseAdapter"/></param>
        /// <returns>A <see cref="RuntimePolicyDeterminationResult"/> containing the resulting <see cref="RuntimePolicy"/></returns>
        public RuntimePolicyDeterminationResult DetermineRuntimePolicy(RuntimeEvent runtimeEvent, RuntimePolicy maximumAllowedPolicy, IRequestResponseAdapter requestResponseAdapter)
        {
            if (maximumAllowedPolicy == RuntimePolicy.Off)
            {
                return new RuntimePolicyDeterminationResult(maximumAllowedPolicy, new RuntimePolicyDeterminationResult.RuntimePolicyDeterminationResultMessage[0]);
            }

            var messages = new List<RuntimePolicyDeterminationResult.RuntimePolicyDeterminationResultMessage>();

            // only run policies for this runtimeEvent
            var policies = AvailableRuntimePolicies.Where(policy => policy.ExecuteOn.HasFlag(runtimeEvent));

            var policyContext = new RuntimePolicyContext(requestResponseAdapter.RequestMetadata, Logger, requestResponseAdapter.RuntimeContext);
            foreach (var policy in policies)
            {
                var policyResult = RuntimePolicy.Off;
                try
                {
                    policyResult = policy.Execute(policyContext);

                    if (policyResult != RuntimePolicy.On)
                    {
                        messages.Add(new RuntimePolicyDeterminationResult.RuntimePolicyDeterminationResultMessage(
                            string.Format("RuntimePolicy '{0}' has been returned by IRuntimePolicy of type '{1}' during RuntimeEvent '{2}'.", policyResult, policy.GetType(), runtimeEvent),
                            false));
                    }
                }
                catch (Exception exception)
                {
                    messages.Add(new RuntimePolicyDeterminationResult.RuntimePolicyDeterminationResultMessage(
                        string.Format("Exception thrown when executing IRuntimePolicy of type '{0}'. The resulting RuntimePolicy will be set to 'Off'. {1}Exception: {2}", policy.GetType(), Environment.NewLine, exception),
                        true));
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

            return new RuntimePolicyDeterminationResult(maximumAllowedPolicy, messages.ToArray());
        }

        /// <summary>
        /// Represents the result of determining a resulting <see cref="RuntimePolicy"/>
        /// </summary>
        public class RuntimePolicyDeterminationResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RuntimePolicyDeterminationResult" />
            /// </summary>
            /// <param name="runtimePolicy">The determined <see cref="IRuntimePolicy"/></param>
            /// <param name="messages">The messages gathered when the resulting <see cref="RuntimePolicy"/> was changed by a <see cref="IRuntimePolicy"/>, if any</param>
            public RuntimePolicyDeterminationResult(RuntimePolicy runtimePolicy, RuntimePolicyDeterminationResultMessage[] messages)
            {
                RuntimePolicy = runtimePolicy;
                Messages = messages;
            }

            /// <summary>
            /// Gets the determined <see cref="RuntimePolicy"/>
            /// </summary>
            public RuntimePolicy RuntimePolicy { get; private set; }

            /// <summary>
            /// Gets the messages gathered when the resulting <see cref="RuntimePolicy"/> was changed by a <see cref="IRuntimePolicy"/>, if any
            /// </summary>
            public RuntimePolicyDeterminationResultMessage[] Messages { get; private set; }

            /// <summary>
            /// Represents a message generated when determining a resulting <see cref="RuntimePolicy"/>
            /// </summary>
            public class RuntimePolicyDeterminationResultMessage
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="RuntimePolicyDeterminationResult" />
                /// </summary>
                /// <param name="message">The message</param>
                /// <param name="isWarning">Indication whether the message is a warning or not</param>
                public RuntimePolicyDeterminationResultMessage(string message, bool isWarning)
                {
                    Message = message;
                    IsWarning = isWarning;
                }

                /// <summary>
                /// Gets the message
                /// </summary>
                public string Message { get; private set; }

                /// <summary>
                /// Gets a boolean indicating whether the message is a warning or not
                /// </summary>
                public bool IsWarning { get; private set; }
            }
        }
    }
}