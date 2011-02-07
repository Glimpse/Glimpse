using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class Form : NameValueCollectionPlugin
    {
        public override string Name
        {
            get { return "Form"; }
        }

        public override object GetData(HttpApplication application)
        {
            return Process(application.Request.Form, application);
        }
    }
}
