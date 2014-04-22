using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Metadata
{
    public class HashMetadata : IMetadata
    {
        public string Key
        {
            get { return "hash"; }
        }

        public object GetMetadata(IConfiguration configuration)
        {
            return configuration.Hash;
        }
    }
}