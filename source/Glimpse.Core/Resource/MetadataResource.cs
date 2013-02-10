using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client all pertinent system configuration information.
    /// </summary>
    public class MetadataResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_metadata";
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
            get { return new[] { ResourceParameter.VersionNumber, ResourceParameter.Callback }; }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        public IResourceResult Execute(IResourceContext context)
        {
            var metadata = context.PersistenceStore.GetMetadata();

            if (metadata == null)
            {
                return new StatusCodeResourceResult(404, "Metadata not found.");
            }

            return new CacheControlDecorator(CacheDuration, CacheSetting.Public, new JsonResourceResult(metadata, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name)));
        }
    }
}