using System.Web;
using Glimpse.WebForms.Extensibility;
using Glimpse.WebForms.Extensions;

namespace Glimpse.WebForms.Plugin
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
