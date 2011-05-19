using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Mvc3.Plumbing;
using Glimpse.WebForms;

namespace Glimpse.Mvc3.Extensions
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

        internal static GlimpseBinderMetadata BinderStore(this ControllerContext controllerContext)
        {
            return BinderStore(controllerContext.HttpContext.Items);
        }

        internal static GlimpseBinderMetadata BinderStore(this HttpContext context)
        {
            return BinderStore(context.Items);
        }

        private static GlimpseBinderMetadata BinderStore(IDictionary items)
        {
            var store = items[GlimpseConstants.BinderStore] as GlimpseBinderMetadata;
            if (store == null) items[GlimpseConstants.BinderStore] = store = new GlimpseBinderMetadata();

            return store;
        }
    }
}
