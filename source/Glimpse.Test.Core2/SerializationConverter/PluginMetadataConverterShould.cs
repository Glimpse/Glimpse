using System;
using Glimpse.Core2.Framework;
using Glimpse.Core2.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core2.SerializationConverter
{
    public class PluginMetadataConverterShould
    {
        [Fact]
        public void ConvertAPluginMetadataObject()
        {
            var metadata = new PluginMetadata{DocumentationUri = "anything"};

            var converter = new PluginMetadataConverter();

            var result = converter.Convert(metadata);

            Assert.True(result.ContainsKey("documentationUri"));
            Assert.False(result.ContainsKey("hasMetadata"));
        }

        [Fact]
        public void ThrowExceptionWithInvalidInput()
        {
            var converter = new PluginMetadataConverter();

            Assert.Throws<InvalidCastException>(()=> converter.Convert("bad input"));
        }
    }
}