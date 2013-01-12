using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class HistoryResource : IResource, IKey
    {
        internal const string InternalName = "glimpse_history";
        internal const string TopKey = "top";

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
            get { return new[] { new ResourceParameterMetadata(TopKey, isRequired: false) }; }
        }
        
        public IResourceResult Execute(IResourceContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            int top;
            int.TryParse(context.Parameters.GetValueOrDefault(TopKey, ifNotFound: "50"), out top);

            var data = context.PersistenceStore.GetTop(top);

            if (data == null)
            {
                return new StatusCodeResourceResult(404, string.Format("No data found in top {0}.", top));
            }

            var requests = data.GroupBy(d => d.ClientId).ToDictionary(group => group.Key, group => group.Select(g => new GlimpseRequestHeaders(g)));
            return new JsonResourceResult(requests, context.Parameters.GetValueOrDefault(ResourceParameter.Callback.Name));
        }
    }
}