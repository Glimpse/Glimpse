using System.Web;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    //TODO: Create AspTab
    public class Request:TabBase<HttpContextBase>, IDocumentation
    {
        public override object GetData(ITabContext context)
        {
            var httpContextBase = context.GetRequestContext<HttpContextBase>();

            return httpContextBase.Request;
        }

        public override string Name
        {
            get { return "Request"; }
        }

        public string DocumentationUri
        {
            get { return "http://localhost/someUrl"; }
        }
    }
}
