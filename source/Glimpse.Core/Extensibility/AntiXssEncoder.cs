using Microsoft.Security.Application;

namespace Glimpse.Core.Extensibility
{
    public class AntiXssEncoder:IHtmlEncoder
    {
        public string HtmlAttributeEncode(string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }
    }
}
