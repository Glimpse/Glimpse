using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.AspNet.Tab
{
    public class Server : AspNetTab, IDocumentation, IKey
    {
        public override string Name
        {
            get { return "Server"; }
        }

        public string Key
        {
            get { return "glimpse_server"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Server-Tab"; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.Request.ServerVariables.ToDictionary();
        }
    }
}
