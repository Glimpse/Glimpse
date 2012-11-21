using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Resource
{
    public class MetadataShould
    {
        [Fact]
        public void Construct()
        {
            IResource metadata = new MetadataResource();

            Assert.NotNull(metadata);
        }

        [Fact]
        public void HaveProperName()
        {
            var metadata = new MetadataResource();
            Assert.Equal("glimpse_metadata", metadata.Name);
        }

        [Fact]
        public void RequireParameterKeys()
        {
            var metadata = new MetadataResource();
            Assert.NotEmpty(metadata.Parameters);
        }

        [Fact]
        public void ReturnResourceResult()
        {
            var metadata = new GlimpseMetadata();

            var storeMock = new Mock<IReadOnlyPersistenceStore>();
            storeMock.Setup(s => s.GetMetadata()).Returns(metadata);

            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.PersistenceStore).Returns(storeMock.Object);
            contextMock.Setup(c => c.Parameters[ResourceParameter.Callback.Name]).Returns("a string");

            var resource = new MetadataResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result);
        }

        [Fact]
        public void Return404ResultIfDataIsMissing()
        {
            var storeMock = new Mock<IReadOnlyPersistenceStore>();
            storeMock.Setup(s => s.GetMetadata()).Returns<GlimpseMetadata>(null);

            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.PersistenceStore).Returns(storeMock.Object);

            var resource = new MetadataResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result);

            var statusCodeResult = result as StatusCodeResourceResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }
    }
}