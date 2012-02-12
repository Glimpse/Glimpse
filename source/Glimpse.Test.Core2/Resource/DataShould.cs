using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Resource;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Resource
{
    public class DataShould
    {
        [Fact]
        public void ProvideProperName()
        {
            var resource = new Data();
            Assert.Equal("data.js", resource.Name);
        }

        [Fact]
        public void ReturnTwoParameterKeys()
        {
            var resource = new Data();
            Assert.Equal(2, resource.ParameterKeys.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new Data();

            Assert.Throws<ArgumentNullException>(()=>resource.Execute(null));
        }

        [Fact]
        public void ReturnJsonResultWithProperRequestId()
        {
            var guid = Guid.Parse("321caff1-f442-4dbb-8c5b-3ed528cde3fd");
            var metadataMock = new Mock<IRequestMetadata>();
            var persistanceStoreMock = new Mock<IReadOnlyPersistanceStore>();
            persistanceStoreMock.Setup(ps => ps.GetByRequestId(guid)).Returns(new GlimpseMetadata(guid, metadataMock.Object, new Dictionary<string, TabResult>(), 0));
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { {ResourceParameterKey.RequestId, guid.ToString()} });
            contextMock.Setup(c => c.PersistanceStore).Returns(persistanceStoreMock.Object);

            var resource = new Data();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as JsonResourceResult);
        }

        [Fact]
        public void ReturnStatusCodeResultWithImproperRequestId()
        {
            var guid = Guid.Parse("321caff1-f442-4dbb-8c5b-3ed528cde3fd");
            var metadataMock = new Mock<IRequestMetadata>();
            var persistanceStoreMock = new Mock<IReadOnlyPersistanceStore>();
            persistanceStoreMock.Setup(ps => ps.GetByRequestId(guid)).Returns(new GlimpseMetadata(guid, metadataMock.Object, new Dictionary<string, TabResult>(), 0));
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { { ResourceParameterKey.RequestId, "Not a real guid" } });
            contextMock.Setup(c => c.PersistanceStore).Returns(persistanceStoreMock.Object);

            var resource = new Data();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }

        [Fact]
        public void ReturnStatusCodeResultWithMissingData()
        {
            var guid = Guid.Parse("321caff1-f442-4dbb-8c5b-3ed528cde3fd");
            var persistanceStoreMock = new Mock<IReadOnlyPersistanceStore>();
            persistanceStoreMock.Setup(ps => ps.GetByRequestId(guid)).Returns<GlimpseMetadata>(null);
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { { ResourceParameterKey.RequestId, guid.ToString() } });
            contextMock.Setup(c => c.PersistanceStore).Returns(persistanceStoreMock.Object);

            var resource = new Data();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }
    }
}