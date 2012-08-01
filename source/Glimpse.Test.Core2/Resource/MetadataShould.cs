using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Resource;
using Glimpse.Core2.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Resource
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

            var storeMock = new Mock<IReadOnlyPersistanceStore>();
            storeMock.Setup(s => s.GetMetadata()).Returns(metadata);

            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.PersistanceStore).Returns(storeMock.Object);
            contextMock.Setup(c => c.Parameters[ResourceParameter.Callback.Name]).Returns("a string");

            var resource = new MetadataResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result);
        }

        [Fact]
        public void Return404ResultIfDataIsMissing()
        {
            var storeMock = new Mock<IReadOnlyPersistanceStore>();
            storeMock.Setup(s => s.GetMetadata()).Returns<GlimpseMetadata>(null);

            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.PersistanceStore).Returns(storeMock.Object);

            var resource = new MetadataResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result);

            var statusCodeResult = result as StatusCodeResourceResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }
    }
}