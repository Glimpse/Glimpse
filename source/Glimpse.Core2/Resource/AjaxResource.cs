using System;
using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.ResourceResult;
using System.Linq;

namespace Glimpse.Core2.Resource
{
    public class AjaxResource:IResource
    {
        internal const string InternalName = "glimpse-ajax";
        private const string ParentRequestKey = "ParentRequestId";

        public AjaxResource()
        {
            Name = InternalName;
        }

        public string Name { get; private set; }

        public IEnumerable<string> ParameterKeys
        {
            get { return new[] {ParentRequestKey, ResourceParameterKey.VersionNumber, ResourceParameterKey.Callback}; }
        }
        
        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Guid parentRequestId;

#if NET35
            if (!Glimpse.Core2.Backport.Net35Backport.TryParseGuid(context.Parameters[ParentRequestKey], out parentRequestId))
                return new StatusCodeResourceResult(404);
#else
            if (!Guid.TryParse(context.Parameters[ParentRequestKey], out parentRequestId))
                return new StatusCodeResourceResult(404);
#endif

            var data = context.PersistanceStore.GetByRequestParentId(parentRequestId);

            if (data == null)
                return new StatusCodeResourceResult(404);

            return new JsonResourceResult(data.Where(r=>r.RequestIsAjax), context.Parameters[ResourceParameterKey.Callback]);

        }
    }
}