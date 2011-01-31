using System.Collections.Generic;
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

        public override IDictionary<string, string> GetData(HttpApplication application)
        {
            return Process(application.Request.ServerVariables, application);
        }
    }
}
