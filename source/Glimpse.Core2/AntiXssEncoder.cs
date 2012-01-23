using Glimpse.Core2.Extensibility;
using Microsoft.Security.Application;

namespace Glimpse.Core2
{
    //TODO: Merge AntiXssLibrary.dll into final Glimpse.Core2.dll
    public class AntiXssEncoder:IHtmlEncoder
    {
        public string HtmlAttributeEncode(string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }
    }
}
