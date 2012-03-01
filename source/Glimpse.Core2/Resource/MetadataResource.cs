using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
{
    public class Metadata : IResource
    {
        internal const string InternalName = "metadata.js";
        private const int CacheDuration = 12960000; //150 days

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<string> ParameterKeys
        {
            get { return Enumerable.Empty<string>(); }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            var metadata = context.PersistanceStore.GetMetadata();

            if (metadata == null)
                return new StatusCodeResourceResult(404);

            return new JsonResourceResult(metadata, "console.log", CacheDuration, CacheSetting.Public);
        }
    }
}