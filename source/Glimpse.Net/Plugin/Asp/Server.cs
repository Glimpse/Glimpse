using System;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class Server : IGlimpsePlugin
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
            throw new NotImplementedException();
        }
    }
}
