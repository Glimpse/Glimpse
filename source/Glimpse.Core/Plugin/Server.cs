using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin]
    internal class Server : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Server"; }
        }

        public object GetData(HttpApplication application)
        {
            return application.Request.ServerVariables.Flatten();
        }

        public void SetupInit(HttpApplication application)
        {
        }
    }
}
