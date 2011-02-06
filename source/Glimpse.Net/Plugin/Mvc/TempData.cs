using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class TempData : DictionaryStringObjectPlugin
    {
        public override string Name
        {
            get { return "TempData"; }
        }

        public override object GetData(HttpApplication application)
        {
            return Process(application.Context.Items[GlimpseConstants.TempData] as IDictionary<string,object>, application);
        }
    }
}
