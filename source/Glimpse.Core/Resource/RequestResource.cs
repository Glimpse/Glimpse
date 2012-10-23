using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class RequestResource:IResource
    {
        internal const string InternalName = "glimpse_request";
        private const int CacheDuration = 12960000; //150 days

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] {ResourceParameter.RequestId, ResourceParameter.VersionNumber, ResourceParameter.Callback}; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Guid requestId;

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseGuid(context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name), out requestId))
                return new StatusCodeResourceResult(404);
#else
            if (!Guid.TryParse(context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name), out requestId))
                return new StatusCodeResourceResult(404);
#endif

            var data = context.PersistenceStore.GetByRequestId(requestId);

            if(data == null)
                return new StatusCodeResourceResult(404);

            return new JsonResourceResult(data, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name), CacheDuration, CacheSetting.Private);
        }
    }
}