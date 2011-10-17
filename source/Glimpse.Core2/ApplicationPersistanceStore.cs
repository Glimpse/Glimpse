using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class ApplicationPersistanceStore : IGlimpsePersistanceStore
    {
        private IDataStore DataStore { get; set; }
        private IList<GlimpseMetadata> GlimpseRequests { get; set; }

        public ApplicationPersistanceStore(IDataStore dataStore)
        {
            DataStore = dataStore;

            var glimpseRequests = DataStore.Get<IList<GlimpseMetadata>>("__GlimpseRequests");
            if (glimpseRequests == null)
            {
                glimpseRequests = new List<GlimpseMetadata>();
                DataStore.Set("__GlimpseRequests", glimpseRequests);
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
    }
}