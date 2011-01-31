using System.Collections.Generic;
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

        public override IDictionary<string, string> GetData(HttpApplication application)
        {
            return Process(application.Request.Form, application);
        }
    }
}
