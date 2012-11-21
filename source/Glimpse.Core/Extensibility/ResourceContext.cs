using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    public class ResourceContext : IResourceContext
    {
        public ResourceContext(IDictionary<string, string> parameters, IReadOnlyPersistenceStore persistenceStore, ILogger logger)
        {
            Parameters = parameters;
            PersistenceStore = persistenceStore;
            Logger = logger;
        }

        public IDictionary<string, string> Parameters { get; set; }

        public IReadOnlyPersistenceStore PersistenceStore { get; set; }

        public ILogger Logger { get; set; }
    }
}