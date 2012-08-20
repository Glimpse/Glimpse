using Glimpse.Core.Extensibility;
using Xunit;

namespace Glimpse.Test.Core
{
    public class AntiXssEncoderShould
    {
        [Fact]
        public void HtmlAttributeEncode()
        {
            IHtmlEncoder encoder = new AntiXssEncoder();

            var input = "This is < an ? Html string";

            string output = encoder.HtmlAttributeEncode(input);

            Assert.Equal("This&#32;is&#32;&lt;&#32;an&#32;?&#32;Html&#32;string", output);
            
        }
    }
}