using Glimpse.Core2.SerializationConverter;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core2.SerializationConverter
{
    public class BooleanConverterShould
    {
        [Theory]
        [InlineData(true, "True")]
        [InlineData(false, "False")]
        public void ConvertToTitleCasedStrings(bool input, string output)
        {
            var converter = new BooleanConverter();

            var result = converter.Convert(input);

            Assert.Equal(output, result);
        }
    }
}