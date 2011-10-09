using System.Web;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Plugin
{
    public class Request:IGlimpsePlugin, IGlimpseHelp
    {
        public object GetData(IServiceLocator locator)
        {
            var httpContextBase = locator.RequestContext as HttpContextBase;

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
