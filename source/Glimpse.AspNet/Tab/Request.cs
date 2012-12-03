using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Request : AspNetTab, IDocumentation, IKey
    {
        public override string Name
        {
            get { return "Request"; }
        }

        public string Key 
        {
            get { return "glimpse_request"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Request"; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            return new RequestModel(httpContext);
        }
    }
}
