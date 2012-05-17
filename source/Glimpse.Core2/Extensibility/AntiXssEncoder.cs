using Microsoft.Security.Application;

namespace Glimpse.Core2.Extensibility
{
    public class AntiXssEncoder:IHtmlEncoder
    {
        public string HtmlAttributeEncode(string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }
    }
}
