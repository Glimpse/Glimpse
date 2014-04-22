using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

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