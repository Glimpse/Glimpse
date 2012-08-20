using System.Text.RegularExpressions;
using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
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