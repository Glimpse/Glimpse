using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    //TODO: Refactor to use Tester pattern
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

            var requestMetadataMock = new Mock<IRequestMetadata>();
            requestMetadataMock.Setup(r => r.RequestHttpMethod).Returns("POST");
            requestMetadataMock.Setup(r => r.RequestUri).Returns("http://localhost");

            var pluginData = new Dictionary<string, string>();

            persistanceStore.Save(new GlimpseMetadata(Guid.NewGuid(), requestMetadataMock.Object, pluginData));

            Assert.Equal(1, persistanceStore.Count());
        }

        [Fact]
        public void GetGlimpseMetadataById()
        {
            IGlimpsePersistanceStore persistanceStore = new ApplicationPersistanceStore(DataStore);

            var requestId = Guid.NewGuid();
            var metadata = new GlimpseMetadata(requestId, new Mock<IRequestMetadata>().Object, new Dictionary<string, string>());

            persistanceStore.Save(metadata);

            var result = persistanceStore.GetById(requestId);

            Assert.Equal(metadata, result);
        }

        [Fact]
        public void GetGlimpseClients()
        {
            IGlimpsePersistanceStore persistanceStore = new ApplicationPersistanceStore(DataStore);

            var clientName = Guid.NewGuid().ToString();
            var requestId = Guid.NewGuid();

            var requestMetadataMock = new Mock<IRequestMetadata>();
            requestMetadataMock.Setup(r => r.GetCookie(Constants.ControlCookieName)).Returns(clientName);

            var metadata = new GlimpseMetadata(requestId, requestMetadataMock.Object, new Dictionary<string, string>());

            persistanceStore.Save(metadata);

            var result = persistanceStore.GetClients();

            Assert.Equal(clientName, result.Keys.First());
            Assert.Equal(1, result[clientName]);
        }

        [Fact]
        public void GetGlipseMetadataByClientName()
        {
            IGlimpsePersistanceStore persistanceStore = new ApplicationPersistanceStore(DataStore);

            var clientName = Guid.NewGuid().ToString();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();

            var requestMetadataMock = new Mock<IRequestMetadata>();
            requestMetadataMock.Setup(r => r.GetCookie(Constants.ControlCookieName)).Returns(clientName);

            var metadata1 = new GlimpseMetadata(requestId1, requestMetadataMock.Object, new Dictionary<string, string>());
            var metadata2 = new GlimpseMetadata(requestId2, requestMetadataMock.Object, new Dictionary<string, string>());

            persistanceStore.Save(metadata1);
            persistanceStore.Save(metadata2);

            var result = persistanceStore.GetByClient(clientName);

            Assert.True(result.Length == 2);
        }

        [Fact(Skip = "Come back to this later")]
        public void GetGlimpseMetadataByParentRequestId()
        {
            Assert.True(false, "Need to implement");
        }

    }
}
