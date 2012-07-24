using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Request:AspNetTab, IDocumentation
    {
        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            return new RequestModel(httpContext);
        }

        public override string Name
        {
            get { return "Request"; }
        }

        public string DocumentationUri
        {
            //TODO: Update to proper Uri
            get { return "http://localhost/someUrl"; }
        }
    }
}
