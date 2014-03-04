using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client all diagnostics information gathered by Glimpse for a specific request.
    /// </summary>
    public class RequestResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_request";
        private const int CacheDuration = 12960000; // 150 days

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// Resource name's should be unique across a given web application. If multiple <see cref="IResource" /> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.
        /// </remarks>
        public string Name
        {
            get { return InternalName; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key. Only valid JavaScript identifiers should be used for future compatibility.
        /// </value>
        public string Key
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get
            {
                return new[] { ResourceParameter.RequestId, ResourceParameter.Hash, ResourceParameter.Callback };
            }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="context"/> is <c>null</c>.</exception>
        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Guid requestId;
            var request = context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name);

#if NET35
            if (!global::Glimpse.Core.Backport.Net35Backport.TryParseGuid(request, out requestId))
            {
                return new StatusCodeResourceResult(404, string.Format("Could not parse RequestId '{0} as GUID.'", request));
            }
#else
            if (!Guid.TryParse(request, out requestId))
            {
                return new StatusCodeResourceResult(404, string.Format("Could not parse RequestId '{0} as GUID.'", request));
            }
#endif

            var data = context.PersistenceStore.GetByRequestId(requestId);

            if (data == null)
            {
                return new StatusCodeResourceResult(404, string.Format("No data found for RequestId '{0}'.", request));
            }

            return new CacheControlDecorator(CacheDuration, CacheSetting.Private, new JsonResourceResult(data, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name)));
        }
    }
}