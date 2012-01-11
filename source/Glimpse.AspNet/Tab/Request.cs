using System.Web;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    [GlimpseTab(RequestContextType=typeof(HttpContextBase))] //TODO: Remove System.ComponentModel.Composition reference
    public class Request:ITab, IGlimpseHelp
    {
        public object GetData(ITabContext context)
        {
            var httpContextBase = context.GetRequestContext<HttpContextBase>();

            return httpContextBase.Request;
        }

        public string Name
        {
            get { return "Request"; }
        }

        public string HelpUri
        {
            get { return "http://localhost/someUrl"; }
        }
    }
}
