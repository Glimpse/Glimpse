using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class ApplicationPersistanceStore : IGlimpsePersistanceStore
    {
        private IDataStore DataStore { get; set; }
        private IList<GlimpseMetadata> GlimpseRequests { get; set; }
        private const string PersistanceStoreKey = "__GlimpsePersistanceKey";

        public ApplicationPersistanceStore(IDataStore dataStore)
        {
            DataStore = dataStore;

            var glimpseRequests = DataStore.Get<IList<GlimpseMetadata>>(PersistanceStoreKey);
            if (glimpseRequests == null)
            {
                glimpseRequests = new List<GlimpseMetadata>();
                DataStore.Set(PersistanceStoreKey, glimpseRequests);
            }
            GlimpseRequests = glimpseRequests;
        }

        public int Count()
        {
            return GlimpseRequests.Count;
        }

        public void Save(GlimpseMetadata data)
        {
            GlimpseRequests.Add(data);
        }

        public GlimpseMetadata GetById(Guid requestId)
        {
            return GlimpseRequests.Where(r => r.RequestId == requestId).First();
        }

        public GlimpseMetadata[] GetByClient(string clientName)
        {
            return GlimpseRequests.Where(r => r.GlimpseClientName.Equals(clientName, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }

        public IDictionary<string, int> GetClients()
        {
            var result = new Dictionary<string, int>();

            foreach (var request in GlimpseRequests)
            {
                if (!result.ContainsKey(request.GlimpseClientName))
                    result.Add(request.GlimpseClientName, 1);
                else
                    result[request.GlimpseClientName]++;
            }

            return result;
        }
    }
}