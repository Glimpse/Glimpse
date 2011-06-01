using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin]
    internal class Server : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public string Name
        {
            get { return "Server"; }
        }

        public object GetData(HttpContextBase context)
        {
            return context.Request.ServerVariables.Flatten();
        }

        public void SetupInit()
        {
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Server"; }
        }
    }
}
