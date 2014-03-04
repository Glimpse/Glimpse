using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client a listing of Ajax requests, filtered by parent request ID.
    /// </summary>
    public class AjaxResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_ajax";
        private const string ParentRequestKey = "parentRequestId";

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
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { new ResourceParameterMetadata(ParentRequestKey), ResourceParameter.Hash, ResourceParameter.Callback }; }
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

            Guid parentRequestId;
            var parentRequestKey = context.Parameters.GetValueOrDefault(ParentRequestKey);

#if NET35
            if (!global::Glimpse.Core.Backport.Net35Backport.TryParseGuid(parentRequestKey, out parentRequestId))
            {
                return new StatusCodeResourceResult(404, string.Format("Could not parse ParentRequestKey '{0}' as GUID.", parentRequestKey));
            }
#else
            if (!Guid.TryParse(parentRequestKey, out parentRequestId))
            {
                return new StatusCodeResourceResult(404, string.Format("Could not parse ParentRequestKey '{0}' as GUID.", parentRequestKey));
            }
#endif

            var data = context.PersistenceStore.GetByRequestParentId(parentRequestId);

            if (data == null)
            {
                return new StatusCodeResourceResult(404, string.Format("Could not find requests with ParentRequestKey '{0}'.", parentRequestKey));
            }

            return new CacheControlDecorator(0, CacheSetting.NoCache, new JsonResourceResult(data.Where(r => r.RequestIsAjax), context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name)));
        }
    }
}