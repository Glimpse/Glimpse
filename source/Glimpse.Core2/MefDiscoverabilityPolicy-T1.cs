using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using Glimpse.Core2.Extensions;

namespace Glimpse.Core2
{
    //TODO: Remove MEF dependency to support .NET 3.5
    public class MefDiscoverabilityPolicy<T>:DiscoverabilityPolicy
    {
        private IList<T> Collection { get; set; }

        public MefDiscoverabilityPolicy(IList<T> innerCollection)
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
