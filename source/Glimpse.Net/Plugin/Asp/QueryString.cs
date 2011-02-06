using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class QueryString : NameValueCollectionPlugin
    {
        public override string Name
        {
            get { return "QueryString"; }
        }

        public override object GetData(HttpApplication application)
        {
            return Process(application.Request.QueryString, application);
        }
    }
}
