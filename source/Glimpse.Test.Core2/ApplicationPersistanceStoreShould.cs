using System;
using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class ApplicationPersistanceStoreShould
    {
        public IDataStore DataStore { get; set; }

        public ApplicationPersistanceStoreShould()
        {
            DataStore = new DictionaryDataStoreAdapter(new Dictionary<object, object>());
        }

        [Fact]
        public void Construct()
        {
            IGlimpsePersistanceStore persistanceStore = new ApplicationPersistanceStore(DataStore);

            Assert.NotNull(persistanceStore);
            Assert.Equal(0, persistanceStore.Count());
        }

        [Fact]
        public void Persist()
        {
            var persistanceStore = new ApplicationPersistanceStore(DataStore);

            Assert.Equal(0, persistanceStore.Count());

            var metadata = new RequestMetadata
                              {
                                  HttpMethod = "POST",
                                  Uri = "http://localhost"
                              };
            var pluginData = new Dictionary<string, string>();

            persistanceStore.Save(new GlimpseMetadata(Guid.NewGuid(), metadata, pluginData));

            Assert.Equal(1, persistanceStore.Count());
        }

        [Fact]
        public void GetGlimpseMetadataById()
        {
            IGlimpsePersistanceStore persistanceStore = new ApplicationPersistanceStore(DataStore);

            var requestId = Guid.NewGuid();
            var metadata = new GlimpseMetadata(requestId, new RequestMetadata(), new Dictionary<string, string>());

            persistanceStore.Save(metadata);

            var result = persistanceStore.GetById(requestId);

            Assert.Equal(metadata, result);
        }
    }
}
