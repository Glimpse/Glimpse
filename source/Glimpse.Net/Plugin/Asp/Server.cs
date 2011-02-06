using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class Server : NameValueCollectionPlugin
    {
        public override string Name
        {
            get { return "Server"; }
        }

        public override object GetData(HttpApplication application)
        {
            return Process(application.Request.ServerVariables, application);
        }
    }
}
