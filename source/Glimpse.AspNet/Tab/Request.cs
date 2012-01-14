using System.Web;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    [Tab(RequestContextType=typeof(HttpContextBase))] //TODO: Remove System.ComponentModel.Composition reference
    public class Request:ITab, IDocumentation
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

        public string DocumentationUri
        {
            get { return "http://localhost/someUrl"; }
        }
    }
}
