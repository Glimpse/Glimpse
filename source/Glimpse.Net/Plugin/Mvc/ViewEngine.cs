using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
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

        public void SetupInit()
        {
            var filters = GlobalFilters.Filters;

            if (!filters.OfType<GlimpseFilterAttribute>().Any())
                filters.Add(new GlimpseFilterAttribute(), int.MinValue);
        }
    }
}
