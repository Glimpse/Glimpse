using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class RequestResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_request";
        private const int CacheDuration = 12960000; // 150 days

        public string Name
        {
            get { return InternalName; }
        }

        public string Key
        {
            get { return Name; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get
            {
                return new[] { ResourceParameter.RequestId, ResourceParameter.VersionNumber, ResourceParameter.Callback };
            }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Guid requestId;
            var request = context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name);

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseGuid(request, out requestId))
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