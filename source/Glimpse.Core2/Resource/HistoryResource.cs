using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;

namespace Glimpse.Core2.Resource
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

            var top = 50;
            int.TryParse(context.Parameters[TopKey], out top);

            var data = context.PersistanceStore.GetTop(top);

            if (data == null)
                return new StatusCodeResourceResult(404);

            return new JsonResourceResult(data.GroupBy(d => d.ClientId).ToDictionary(group => group.Key, group => group.Cast<IRequestMetadata>()), context.Parameters[ResourceParameter.Callback.Name]);
        }
    }
}