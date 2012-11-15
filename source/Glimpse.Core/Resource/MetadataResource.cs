using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class MetadataResource : IResource
    {
        internal const string InternalName = "glimpse_metadata";
        private const int CacheDuration = 12960000; // 150 days

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.VersionNumber, ResourceParameter.Callback }; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            var metadata = context.PersistenceStore.GetMetadata();

            if (metadata == null)
            {
                return new StatusCodeResourceResult(404);
            }

            return new CacheControlDecorator(CacheDuration, CacheSetting.Public, new JsonResourceResult(metadata, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name)));
        }
    }
}