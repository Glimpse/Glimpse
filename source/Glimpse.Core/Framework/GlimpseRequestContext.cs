using System;
using System.Collections.Generic;
using System.Diagnostics;
using Glimpse.Core.Extensibility;
#if NET35
using Glimpse.Core.Backport;
#endif

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents the context of a specific request, which is used as an access point to the request's <see cref="IRequestResponseAdapter"/> handle
    /// </summary>
    internal sealed class GlimpseRequestContext : IGlimpseRequestContext
    {
        private RuntimePolicyDeterminator RuntimePolicyDeterminator { get; set; }

        private RuntimePolicy currentRuntimePolicy;
        private IExecutionTimer activeExecutionTimer;

        private Stopwatch GlobalStopwatch { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestContext" />
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        /// <param name="requestResponseAdapter">The <see cref="IRequestResponseAdapter "/> of this request.</param>
        /// <param name="initialRuntimePolicy">The initial <see cref="RuntimePolicy "/> for this request.</param>
        /// <param name="resourceEndpointConfiguration">The <see cref="IResourceEndpointConfiguration"/>.</param>
        /// <param name="endpointBaseUri">The endpoint base URI.</param>
        /// <param name="runtimePolicyDeterminator">The runtime policy determinator</param>
        /// <param name="glimpseScriptTagsGenerator">The Glimpse script tags generator</param>
        /// <param name="logger">The logger</param>
        public GlimpseRequestContext(
            Guid glimpseRequestId,
            IRequestResponseAdapter requestResponseAdapter,
            RuntimePolicy initialRuntimePolicy,
            IResourceEndpointConfiguration resourceEndpointConfiguration,
            string endpointBaseUri,
            RuntimePolicyDeterminator runtimePolicyDeterminator,
            IGlimpseScriptTagsGenerator glimpseScriptTagsGenerator,
            ILogger logger)
        {
            Guard.ArgumentNotNull("requestResponseAdapter", requestResponseAdapter);
            Guard.ArgumentNotNull("resourceEndpointConfiguration", resourceEndpointConfiguration);
            Guard.ArgumentNotNull("runtimePolicyDeterminator", runtimePolicyDeterminator);
            Guard.ArgumentNotNull("glimpseScriptTagsGenerator", glimpseScriptTagsGenerator);

            if (string.IsNullOrEmpty(endpointBaseUri))
            {
                throw new ArgumentException("endpointBaseUri is null or empty");
            }

            GlimpseRequestId = glimpseRequestId;
            RequestResponseAdapter = requestResponseAdapter;
            RuntimePolicyDeterminator = runtimePolicyDeterminator;

#warning CGI - maybe a factory would be cleaner instead of needing to accept passthrough parameters
            ScriptTagsProvider = new GlimpseScriptTagsProvider(GlimpseRequestId, glimpseScriptTagsGenerator, logger, IsAllowedToProvideScriptTags);

            RequestHandlingMode = resourceEndpointConfiguration.IsResourceRequest(requestResponseAdapter.RequestMetadata.RequestUri, endpointBaseUri)
                                    ? RequestHandlingMode.ResourceRequest
                                    : RequestHandlingMode.RegularRequest;

            RequestStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
            this.currentRuntimePolicy = initialRuntimePolicy;
        }

        /// <summary>
        /// Gets the Glimpse Id assigned to this request
        /// </summary>
        public Guid GlimpseRequestId { get; private set; }

        /// <summary>
        /// Gets the <see cref="IRequestResponseAdapter"/> for the referenced request
        /// </summary>
        public IRequestResponseAdapter RequestResponseAdapter { get; private set; }

        /// <summary>
        /// Gets the <see cref="IDataStore"/> for this request
        /// </summary>
        public IDataStore RequestStore { get; private set; }

        /// <summary>
        /// Gets or sets the active <see cref="RuntimePolicy"/> for the referenced request
        /// </summary>
        public RuntimePolicy CurrentRuntimePolicy
        {
            get
            {
                return this.currentRuntimePolicy;
            }

            set
            {
                if (value > this.currentRuntimePolicy)
                {
                    throw new GlimpseException("You're not allowed to increase the active runtime policy level from '" + this.currentRuntimePolicy + "' to '" + value + "'.");
                }

                this.currentRuntimePolicy = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="RequestHandlingMode"/> for the referenced request
        /// </summary>
        public RequestHandlingMode RequestHandlingMode { get; private set; }

        /// <summary>
        /// Gets the <see cref="GlimpseScriptTagsProvider"/> for the referenced request
        /// </summary>
        public IGlimpseScriptTagsProvider ScriptTagsProvider { get; private set; }

        /// <summary>
        /// Gets the <see cref="IExecutionTimer"/> for the referenced request
        /// </summary>
        public IExecutionTimer CurrentExecutionTimer
        {
            get
            {
                if (activeExecutionTimer == null)
                {
                    throw new GlimpseException("Execution timer is not available, did you start timing?");
                }

                return activeExecutionTimer;
            }

            private set
            {
                activeExecutionTimer = value;
            }
        }

        /// <summary>
        /// Starts timing the execution of the referenced request
        /// </summary>
        public void StartTiming()
        {
            if (GlobalStopwatch != null)
            {
                throw new GlimpseException("Timing already started");
            }

            GlobalStopwatch = Stopwatch.StartNew();
            CurrentExecutionTimer = new ExecutionTimer(GlobalStopwatch);
        }

        /// <summary>
        /// Stops timing the execution of the referenced request
        /// </summary>
        /// <returns>The elapsed time since the start of the timing</returns>
        public TimeSpan StopTiming()
        {
            GlobalStopwatch.Stop();
            return GlobalStopwatch.Elapsed;
        }

        private bool IsAllowedToProvideScriptTags()
        {
            // we reuse the same runtime event but were are not causing side effects as the result is not stored in the current runtime policy
            var policy = RuntimePolicyDeterminator.DetermineRuntimePolicy(RuntimeEvent.EndRequest, CurrentRuntimePolicy, RequestResponseAdapter);
            return policy.HasFlag(RuntimePolicy.DisplayGlimpseClient);
        }
    }
}