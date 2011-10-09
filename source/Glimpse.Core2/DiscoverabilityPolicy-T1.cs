using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;

namespace Glimpse.Core2
{
    public class DiscoverabilityPolicy<T>:DiscoverabilityPolicy
    {
        private IList<T> Collection { get; set; }

        public DiscoverabilityPolicy(IList<T> innerCollection)
        {
            Collection = innerCollection;
        }

        public override void Discover()
        {
            var batch = new CompositionBatch();

            var directoryCatalog = new FilteredSafeDirectoryCatalog(Path, IgnoredTypes);
            var container = new CompositionContainer(directoryCatalog);

            container.Compose(batch);

            var exportedValues = container.GetExportedValues<T>();
            Collection.Clear();
            Collection.AddRange(exportedValues);
        }
    }
}
