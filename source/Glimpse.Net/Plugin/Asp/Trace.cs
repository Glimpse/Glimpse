using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin]
    public class Trace : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Trace"; }
        }

        public object GetData(HttpApplication application)
        {
            return application.Context.Items[GlimpseConstants.TraceMessages];
        }
    }
}