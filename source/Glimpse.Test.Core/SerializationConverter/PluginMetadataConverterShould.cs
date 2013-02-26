using System;
using System.Collections.Generic;
using Glimpse.Core.Framework;
using Glimpse.Core.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core.SerializationConverter
{
    public class PluginMetadataConverterShould
    {
        [Fact]
        public void ConvertAPluginMetadataObject()
        {
            var metadata = new TabMetadata{DocumentationUri = "anything"};

            var converter = new TabMetadataConverter();

            var obj = converter.Convert(metadata);

            var result = obj as IDictionary<string, object>;

            Assert.NotNull(result);
            Assert.True(result.ContainsKey("documentationUri"));
            Assert.False(result.ContainsKey("hasMetadata"));
        }

        [Fact]
        public void ThrowExceptionWithInvalidInput()
        {
            var converter = new TabMetadataConverter();

            Assert.Throws<InvalidCastException>(()=> converter.Convert("bad input"));
        }
    }
}