using System.Collections.Generic;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public class ResourceContext : IResourceContext
    {
        public ResourceContext(IDictionary<string, string> parameters, IReadOnlyPersistanceStore persistanceStore, ILogger logger)
        {
            Parameters = parameters;
            PersistanceStore = persistanceStore;
            Logger = logger;
        }

        public IDictionary<string, string> Parameters { get; set; }

        public IReadOnlyPersistanceStore PersistanceStore { get; set; }

        public ILogger Logger { get; set; }
    }
}