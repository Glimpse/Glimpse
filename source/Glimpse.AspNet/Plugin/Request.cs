using Glimpse.AspNet.Extensibility;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Plugin
{
    public class Request:AspNetGlimpsePlugin, IGlimpseHelp
    {
        public override object GetData(IServiceLocator locator)
        {
            var httpContextBase = locator.GetRequestContext();

            return httpContextBase.Request;
        }

        public override string Name
        {
            get { return "Request"; }
        }

        public string Uri
        {
            get { return "http://localhost/someUrl"; }
        }
    }
}
