using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents the context of a specific request, which is used as an access point to the request's <see cref="IRequestResponseAdapter"/> handle
    /// </summary>
    public class GlimpseRequestContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestContext" />
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        /// <param name="requestResponseAdapter">The <see cref="IRequestResponseAdapter "/> of this request.</param>
        /// <param name="initialRuntimePolicy">The initial <see cref="RuntimePolicy "/> for this request.</param>
        public GlimpseRequestContext(Guid glimpseRequestId, IRequestResponseAdapter requestResponseAdapter, RuntimePolicy initialRuntimePolicy)
            : this(glimpseRequestId, requestResponseAdapter, initialRuntimePolicy, GlimpseRuntime.Instance.Configuration.EndpointBaseUri)
        {
        }

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
            RequestStore.Set(Constants.RuntimePolicyKey, initialRuntimePolicy);
        }

        /// <summary>
        /// Gets the Glimpse Id assigned to this request
        /// </summary>
        public Guid GlimpseRequestId { get; private set; }

        /// <summary>
        /// Gets the <see cref="IRequestResponseAdapter"/> for this request
        /// </summary>
        public IRequestResponseAdapter RequestResponseAdapter { get; private set; }

        /// <summary>
        /// Gets the <see cref="IDataStore"/> for this request
        /// </summary>
        public IDataStore RequestStore { get; private set; }

        /// <summary>
        /// Gets or sets the active <see cref="RuntimePolicy"/> for this request
        /// </summary>
        public virtual RuntimePolicy ActiveRuntimePolicy
        {
#warning CGI: Maybe just rename to CurrentRuntimePolicy?
            get
            {
                return RequestStore.Get<RuntimePolicy>(Constants.RuntimePolicyKey);
            }

            set
            {
                if (value > ActiveRuntimePolicy)
                {
                    throw new GlimpseException("You're not allowed to increase the active runtime policy level from '"  + ActiveRuntimePolicy + "' to '" + value + "'.");
                }

                RequestStore.Set(Constants.RuntimePolicyKey, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="RequestHandlingMode"/> for this request
        /// </summary>
        public virtual RequestHandlingMode RequestHandlingMode { get; private set; }
    }
}