using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class DataShould
    {
        [Fact]
        public void ProvideProperName()
        {
            var resource = new RequestResource();
            Assert.Equal("glimpse_request", resource.Name);
        }

        [Fact]
        public void ReturnThreeParameterKeys()
        {
            var resource = new RequestResource();
            Assert.Equal(3, resource.Parameters.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new RequestResource();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null));
        }

        [Fact]
        public void ReturnJsonResultWithProperRequestId()
        {
            var guid = Guid.Parse("321caff1-f442-4dbb-8c5b-3ed528cde3fd");
            var metadataMock = new Mock<IRequestMetadata>();
            var persistenceStoreMock = new Mock<IReadOnlyPersistenceStore>();
            persistenceStoreMock.Setup(ps => ps.GetByRequestId(guid)).Returns(new GlimpseRequest(guid, metadataMock.Object, new Dictionary<string, TabResult>(), 0));
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { { ResourceParameter.RequestId.Name, guid.ToString() }, { ResourceParameter.Callback.Name, "console.log" } });
            contextMock.Setup(c => c.PersistenceStore).Returns(persistenceStoreMock.Object);

            var resource = new RequestResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result);
        }

        [Fact]
        public void ReturnStatusCodeResultWithImproperRequestId()
        {
            var guid = Guid.Parse("321caff1-f442-4dbb-8c5b-3ed528cde3fd");
            var metadataMock = new Mock<IRequestMetadata>();
            var persistenceStoreMock = new Mock<IReadOnlyPersistenceStore>();
            persistenceStoreMock.Setup(ps => ps.GetByRequestId(guid)).Returns(new GlimpseRequest(guid, metadataMock.Object, new Dictionary<string, TabResult>(), 0));
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { { ResourceParameter.RequestId.Name, "Not a real guid" } });
            contextMock.Setup(c => c.PersistenceStore).Returns(persistenceStoreMock.Object);

            var resource = new RequestResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }

        [Fact]
        public void ReturnStatusCodeResultWithMissingData()
        {
            var guid = Guid.Parse("321caff1-f442-4dbb-8c5b-3ed528cde3fd");
            var persistenceStoreMock = new Mock<IReadOnlyPersistenceStore>();
            persistenceStoreMock.Setup(ps => ps.GetByRequestId(guid)).Returns<GlimpseRequest>(null);
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { { ResourceParameter.RequestId.Name, guid.ToString() } });
            contextMock.Setup(c => c.PersistenceStore).Returns(persistenceStoreMock.Object);

            var resource = new RequestResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }
    }
}