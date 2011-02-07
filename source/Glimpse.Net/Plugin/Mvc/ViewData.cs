using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class ViewData : DictionaryStringObjectPlugin
    {
        public override string Name
        {
            get { return "ViewData"; }
        }

        public override object GetData(HttpApplication application)
        {
            return Process(application.Context.Items[GlimpseConstants.ViewData] as IDictionary<string, object>, application);
        }
    }
}
