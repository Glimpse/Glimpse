using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class MetadataCollection : DiscoverableCollection<IMetadata>
    {
        public MetadataCollection(
            CollectionConfiguration configuration, 
            ILogger logger) : base(configuration, logger)
        {
        }
    }
}
