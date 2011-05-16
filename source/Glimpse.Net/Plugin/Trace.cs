using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Net;
using Glimpse.WebForms.Extensibility;
using Glimpse.WebForms.Plumbing;

namespace Glimpse.WebForms.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Trace : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Trace"; }
        }

        public object GetData(HttpApplication application)
        {
            var messages = application.Context.Items[GlimpseConstants.TraceMessages] as IList<IList<string>>;
            if (messages == null) return null;

            foreach (var message in messages)
            {
                //Add style if the category is recognized
                switch (message[1].ToLower())
                {
                    case "warning":
                    case "warn":
                        message.Add("warn");
                        break;
                    case "information":
                    case "info":
                        message.Add("info");
                        break;
                    case "error":
                        message.Add("error");
                        break;
                    case "fail":
                        message.Add("fail");
                        break;
                    case "quiet":
                        message.Add("quiet");
                        break;
                    case "timing":
                        message.Add("loading");
                        break;
                    case "selected":
                        message.Add("selected");
                        break;
                }
            }

            return messages;
        }

        public void SetupInit(HttpApplication application)
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<GlimpseTraceListener>().Any())
                traceListeners.Add(new GlimpseTraceListener()); //Add trace listener if it isn't already configured
        }
    }
}