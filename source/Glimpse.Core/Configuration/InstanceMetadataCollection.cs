using System;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class InstanceMetadataCollection : DiscoverableCollection<IInstanceMetadata>
    {
        public InstanceMetadataCollection(
            CollectionConfiguration configuration,
            ILogger logger,
            EventHandler onChange = null)
            : base(configuration, logger, onChange)
        {
        }
    }
}
