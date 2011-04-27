using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Extensions
{
    internal static class ControllerContextExtensions
    {
        internal static IList<GlimpseFilterCallMetadata> FiltersStore(this ControllerContext controllerContext)
        {
            var allFilters = controllerContext.HttpContext.Items[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;
            if (allFilters == null) controllerContext.HttpContext.Items[GlimpseConstants.AllFilters] = allFilters = new List<GlimpseFilterCallMetadata>();

            return allFilters;
        }

        internal static IList<GlimpseFilterCalledMetadata> CallStore(this ControllerContext controllerContext)
        {
            var items = controllerContext.HttpContext.Items;
            var store = items[GlimpseConstants.CalledFilters] as IList<GlimpseFilterCalledMetadata>;
            if (store == null) items[GlimpseConstants.CalledFilters] = store = new List<GlimpseFilterCalledMetadata>();

            return store;
        }
    }
}
