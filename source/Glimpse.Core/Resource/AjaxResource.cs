using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class AjaxResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_ajax";
        private const string ParentRequestKey = "parentRequestId";

        public string Name 
        {
            get { return InternalName; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { new ResourceParameterMetadata(ParentRequestKey), ResourceParameter.VersionNumber, ResourceParameter.Callback }; }
        }

        public string Key 
        {
            get { return Name; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Guid parentRequestId;

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseGuid(context.Parameters[ParentRequestKey], out parentRequestId))
            {
                return new StatusCodeResourceResult(404);
            }
#else
            if (!Guid.TryParse(context.Parameters.GetValueOrDefault(ParentRequestKey), out parentRequestId))
            {
                return new StatusCodeResourceResult(404);
            }
#endif

            var data = context.PersistenceStore.GetByRequestParentId(parentRequestId);

            if (data == null)
            {
                return new StatusCodeResourceResult(404);
            }

            return new JsonResourceResult(data.Where(r => r.RequestIsAjax), context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name));
        }
    }
}