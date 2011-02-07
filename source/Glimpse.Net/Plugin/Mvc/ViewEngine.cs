using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class ViewEngine:IGlimpsePlugin
    {
        public string Name
        {
            get { return "ViewEngines"; }
        }

        public object GetData(HttpApplication application)
        {
            var viewEngines = new Dictionary<string, string>();
            var counter = 1;
            foreach (var item in ViewEngines.Engines)
            {
                viewEngines.Add(counter++.ToString(), item.GetType().ToString());
            }

            if (viewEngines.Count == 0) return null;

            return viewEngines;
        }
    }
}
