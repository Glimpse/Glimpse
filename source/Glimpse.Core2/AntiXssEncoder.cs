using Glimpse.Core2.Extensibility;
using Microsoft.Security.Application;

namespace Glimpse.Core2
{
    //TODO: Merge in AntiXss library
    public class AntiXssEncoder:IHtmlEncoder
    {
        public string HtmlAttributeEncode(string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }
    }
}
