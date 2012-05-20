using System.Text.RegularExpressions;
using Glimpse.Core2.Configuration;
using Xunit;

namespace Glimpse.Test.Core2.Configuration
{
    public class RegexElementShould
    {
        [Fact]
        public void GetSetRegex()
        {
            var element = new RegexElement();

            var regex = new Regex(".+");

            element.Regex = regex;

            Assert.Equal(regex, element.Regex);
        }
    }
}