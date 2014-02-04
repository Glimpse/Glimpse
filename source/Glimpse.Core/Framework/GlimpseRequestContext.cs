using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents the context of a specific request, which is used as an access point to the request's <see cref="IRequestResponseAdapter"/> handle
    /// </summary>
    internal sealed class GlimpseRequestContext : IGlimpseRequestContext
    {
        private RuntimePolicy currentRuntimePolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestContext" />
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        /// <param name="requestResponseAdapter">The <see cref="IRequestResponseAdapter "/> of this request.</param>
        /// <param name="initialRuntimePolicy">The initial <see cref="RuntimePolicy "/> for this request.</param>
        /// <param name="endpointBaseUri">The endpoint base URI.</param>
        public GlimpseRequestContext(Guid glimpseRequestId, IRequestResponseAdapter requestResponseAdapter, RuntimePolicy initialRuntimePolicy, string endpointBaseUri)
        {
            if (requestResponseAdapter == null)
            {
                throw new ArgumentNullException("requestResponseAdapter");
            }

            GlimpseRequestId = glimpseRequestId;
            RequestResponseAdapter = requestResponseAdapter;
            RequestHandlingMode = RequestResponseAdapter.RequestMetadata.AbsolutePath.StartsWith(endpointBaseUri, StringComparison.InvariantCultureIgnoreCase)
                                    || ("~" + RequestResponseAdapter.RequestMetadata.AbsolutePath).StartsWith(endpointBaseUri, StringComparison.InvariantCultureIgnoreCase)
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
    }
}