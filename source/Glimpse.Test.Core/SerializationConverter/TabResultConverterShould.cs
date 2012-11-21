using System.Collections.Generic;
using Glimpse.Core.Framework;
using Glimpse.Core.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core.SerializationConverter
{
    public class TabResultConverterShould
    {
        [Fact]
        public void ConvertToValidJson()
        {
            var converter = new TabResultConverter();
            var obj = converter.Convert(new TabResult("XYZ", new{Any="Object"}));

            var result = obj as IDictionary<string, object>;

            Assert.NotNull(result);
            Assert.True(result.ContainsKey("data"));
            Assert.True(result.ContainsKey("name"));
        }
    }
}