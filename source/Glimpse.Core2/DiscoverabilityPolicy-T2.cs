using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace Glimpse.Core2
{
    public class DiscoverabilityPolicy<TPart, TPartMetadata> : DiscoverabilityPolicy
    {
        private IList<Lazy<TPart, TPartMetadata>> Collection { get; set; }

        public DiscoverabilityPolicy(IList<Lazy<TPart, TPartMetadata>> collection)
        {
            Collection = collection;
        }

        public override void Discover()
        {
            var batch = new CompositionBatch();

            var directoryCatalog = new FilteredSafeDirectoryCatalog(Path, IgnoredTypes);
            var container = new CompositionContainer(directoryCatalog);

            container.Compose(batch);

            var exportedValues = container.GetExports<TPart, TPartMetadata>();

            Collection.Clear();
            Collection.AddRange(exportedValues);
        }
    }
}