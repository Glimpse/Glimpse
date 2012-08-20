using System.Text.RegularExpressions;
using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class RegexConverterShould
    {
        [Fact]
        public void ConvertStringToRegex()
        {
            var rString = ".+";

            var converter = new RegexConverter();

            var result = converter.ConvertFrom(rString);

            var rResult = result as Regex;

            Assert.NotNull(rResult);
            Assert.Equal(rString, rResult.ToString());
        }

        [Fact]
        public void ConvertRegexToString()
        {

            var rString = ".+";
            var regex = new Regex(rString);


            var converter = new RegexConverter();

            var result = converter.ConvertTo(regex, typeof (string));
            var sResult = result as string;

            Assert.False(string.IsNullOrEmpty(sResult));
            Assert.Equal(rString, sResult);
        }
    }
}