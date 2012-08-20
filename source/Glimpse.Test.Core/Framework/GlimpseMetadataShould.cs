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

            var pluginData = new Dictionary<string, TabResult>();
            var expectedDuration = 5;
            var metadata = new GlimpseRequest(guid, metadataMock.Object, pluginData, expectedDuration);

            Assert.Equal(guid, metadata.RequestId);
            Assert.Equal(pluginData, metadata.PluginData);
            Assert.Equal(expectedDuration, metadata.Duration);
            Assert.Null(metadata.ParentRequestId);
        }
    }
}