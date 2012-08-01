using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Resource
{
    public class ConfigurationResource:IResource
    {
        internal const string InternalName = "glimpse_config";

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return Enumerable.Empty<ResourceParameterMetadata>(); }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            //TODO: Implement Me
            throw new System.NotImplementedException();
        }
    }
}