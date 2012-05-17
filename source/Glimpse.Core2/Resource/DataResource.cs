using System;
using System.Collections.Generic;
#if NET35
using Glimpse.Core2.Backport;
#endif
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
{
    public class Data:IResource
    {
        internal const string InternalName = "data.js";
        private const int CacheDuration = 12960000; //150 days

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<string> ParameterKeys
        {
            get { return new[] {ResourceParameterKey.RequestId, ResourceParameterKey.VersionNumber, ResourceParameterKey.Callback}; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Guid requestId;

#if NET35
            if (!Net35Backport.TryParseGuid(context.Parameters[ResourceParameterKey.RequestId], out requestId))
                return new StatusCodeResourceResult(404);
#else
            if (!Guid.TryParse(context.Parameters[ResourceParameterKey.RequestId], out requestId))
                return new StatusCodeResourceResult(404);
#endif

            var data = context.PersistanceStore.GetByRequestId(requestId);

            if(data == null)
                return new StatusCodeResourceResult(404);

            return new JsonResourceResult(data, context.Parameters[ResourceParameterKey.Callback], CacheDuration, CacheSetting.Private);
        }
    }
}