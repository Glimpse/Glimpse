using System;
using System.Collections;
using System.Collections.Generic;
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
            var messages = application.Context.Items[GlimpseConstants.TraceMessages] as IList<IList<string>>;
            if (messages == null) return null;

            foreach (var message in messages)
            {
                switch (message[1])
                {
                    case "Warning":
                        message.Add("Warn");
                        break;
                    case "Information":
                        message.Add("Info");
                        break;
                    case "Error":
                        message.Add("Error");
                        break;
                    case "Fail":
                        message.Add("Fail");
                        break;
                    case "Quiet ":
                        message.Add("Quiet");
                        break;
                    case "Selected":
                        message.Add("Selected");
                        break;

                }
            }

            return messages;
        }
    }
}