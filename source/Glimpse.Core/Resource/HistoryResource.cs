using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class HistoryResource:IResource
    {
        internal const string InternalName = "glimpse_history";
        internal const string TopKey = "top";

        public HistoryResource()
        {
            Name = InternalName;
        }

        public string Name { get; private set; }

        public IEnumerable<ResourceParameterMetadata> Parameters {
            get { return new[] { new ResourceParameterMetadata(TopKey, isRequired:false) }; }
        }
        
        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var top = 0;
            int.TryParse(context.Parameters.GetValueOrDefault(TopKey, ifNotFound:"50"), out top);

            var data = context.PersistanceStore.GetTop(top);

            if (data == null)
                return new StatusCodeResourceResult(404);

            var requests = data.GroupBy(d => d.ClientId).ToDictionary(group => group.Key, group => group.Select(g=>new GlimpseRequestHeaders(g)));
            return new JsonResourceResult(requests, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name));
        }
    }
}