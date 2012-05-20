using Glimpse.Core2.Framework;
using Glimpse.Core2.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core2.SerializationConverter
{
    public class TabResultConverterShould
    {
        [Fact]
        public void ConvertToValidJson()
        {
            var converter = new TabResultConverter();
            var result = converter.Convert(new TabResult("XYZ", new{Any="Object"}));

            Assert.True(result.ContainsKey("data"));
            Assert.True(result.ContainsKey("name"));
        }
    }
}