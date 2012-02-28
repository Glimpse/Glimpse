using System;
using System.Collections.Generic;
using Glimpse.Core2.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Framework
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
            var metadata = new GlimpseMetadata(guid, metadataMock.Object, pluginData, expectedDuration);

            Assert.Equal(guid, metadata.RequestId);
            Assert.Equal(pluginData, metadata.PluginData);
            Assert.Equal(expectedDuration, metadata.Duration);
            Assert.Equal(default(Guid), metadata.ParentRequestId);
        }
    }
}