using Glimpse.Core2.Framework;
using Glimpse.Core2.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core2.SerializationConverter
{
    public class GlimpseMetadataConverterShould
    {
        [Fact]
        public void ConvertAGlimpseMetadataObject()
        {
            var metadata = new GlimpseMetadata();

            var converter = new GlimpseMetadataConverter();

            var result = converter.Convert(metadata);

            Assert.True(result.ContainsKey("version"));
            Assert.True(result.ContainsKey("plugins"));
            Assert.True(result.ContainsKey("paths"));
        }
    }
}