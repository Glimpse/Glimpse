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
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client a listing of historical requests, grouped by client.
    /// </summary>
    public class HistoryResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_history";
        internal const string TopKey = "top";

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
            get { return new[] { new ResourceParameterMetadata(TopKey, isRequired: false) }; }
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

            int top;
            int.TryParse(context.Parameters.GetValueOrDefault(TopKey, ifNotFound: "50"), out top);

            var data = context.PersistenceStore.GetTop(top);

            if (data == null)
            {
                return new StatusCodeResourceResult(404, string.Format("No data found in top {0}.", top));
            }

            var requests = data.GroupBy(d => d.ClientId).ToDictionary(group => group.Key, group => group.Select(g => new GlimpseRequestHeaders(g)));
            return new CacheControlDecorator(0, CacheSetting.NoCache, new JsonResourceResult(requests, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name)));
        }
    }
}