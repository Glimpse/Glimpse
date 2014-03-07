using System;
using System.Collections.Generic;
using Glimpse.Core.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseMetadataShould
    {
        [Fact]
        public void Construct()
        {
            var guid = Guid.NewGuid();
            var metadataMock = new Mock<IRequestMetadata>();
            metadataMock.Setup(requestMetadata => requestMetadata.RequestUri).Returns(new Uri("http://localhost"));

            var instanceMetadata = new Dictionary<string, object>();
            var pluginData = new Dictionary<string, TabResult>();
            var expectedDuration = TimeSpan.FromMilliseconds(5);
            var metadata = new GlimpseRequest(guid, metadataMock.Object, pluginData, pluginData, expectedDuration, instanceMetadata);

            Assert.Equal(guid, metadata.RequestId);
            Assert.Equal(pluginData, metadata.TabData);
            Assert.Equal(expectedDuration, metadata.Duration);
            Assert.Equal(instanceMetadata, metadata.Metadata);
            Assert.Null(metadata.ParentRequestId);
        }
    }
}