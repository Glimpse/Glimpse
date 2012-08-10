using System;
using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Extensions;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
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
            if (!Glimpse.Core2.Backport.Net35Backport.TryParseGuid(context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name), out requestId))
                return new StatusCodeResourceResult(404);
#else
            if (!Guid.TryParse(context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name), out requestId))
                return new StatusCodeResourceResult(404);
#endif

            var data = context.PersistanceStore.GetByRequestId(requestId);

            if(data == null)
                return new StatusCodeResourceResult(404);

            return new JsonResourceResult(data, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name), CacheDuration, CacheSetting.Private);
        }
    }
}