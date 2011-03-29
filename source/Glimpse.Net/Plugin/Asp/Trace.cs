using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Asp
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Trace : IGlimpsePlugin
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
                switch (message[1])
                {
                    case "Warning":
                    case "Warn":
                        message.Add("warn");
                        break;
                    case "Information":
                    case "Info":
                        message.Add("info");
                        break;
                    case "Error":
                        message.Add("error");
                        break;
                    case "Fail":
                        message.Add("fail");
                        break;
                    case "Quiet ":
                        message.Add("quiet");
                        break;
                    case "Selected":
                        message.Add("selected");
                        break;
                }
            }

            return messages;
        }

        public void SetupInit()
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<GlimpseTraceListener>().Any())
                traceListeners.Add(new GlimpseTraceListener()); //Add trace listener if it isn't already configured
        }
    }
}