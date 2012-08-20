using Glimpse.Core.SerializationConverter;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.SerializationConverter
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