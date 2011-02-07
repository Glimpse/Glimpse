using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class GlobalFilters:IGlimpsePlugin
    {
        public string Name
        {
            get { return "GlobalFilters"; }
        }

        public IDictionary<string, string> GetData(HttpApplication application)
        {
            var globalFilters = new Dictionary<string, string>();
            foreach (Filter item in System.Web.Mvc.GlobalFilters.Filters)
            {
                var key = item.Order.ToString();
                if (!globalFilters.ContainsKey(key))
                    globalFilters.Add(item.Order.ToString(), item.Instance.ToString());
            }

            if (globalFilters.Count == 0) return null;


            return globalFilters;
        }
    }
}
