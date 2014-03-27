using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class VersionMetadata : IMetadata
    {
        public string Key
        {
            get { return "version"; }
        }

        public object GetMetadata(IConfiguration configuration)
        {
            return configuration.Version;
        }
    }
}
