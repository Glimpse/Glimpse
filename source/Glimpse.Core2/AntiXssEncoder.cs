using Glimpse.Core2.Extensibility;
using Microsoft.Security.Application;

namespace Glimpse.Core2
{
    public class AntiXssEncoder:IGlimpseHtmlEncoder
    {
        public string HtmlAttributeEncode(string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }
    }
}
