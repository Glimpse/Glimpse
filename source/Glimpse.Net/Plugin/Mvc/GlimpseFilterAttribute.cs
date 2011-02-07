using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace Glimpse.Net.Mvc
{
    public class GlimpseFilterAttribute: ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Add(GlimpseConstants.Result, filterContext.Result);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Add(GlimpseConstants.TempData, filterContext.Controller.TempData);
            store.Add(GlimpseConstants.ViewData, filterContext.Controller.ViewData);
            store.Add(GlimpseConstants.ActionDescriptor, filterContext.ActionDescriptor);

            AddFilters(store, filterContext);
        }

        private static void AddFilters(IDictionary store, ActionExecutedContext context)
        {
            //FilterProviders.Providers.GetFilters aggregates all filters from:
            //- GloalFilters.Filters
            //- FilterAttributeFilterProvider
            //- ControllerInstanceFilterProvider
            //- DependancyResolver

            var filters = FilterProviders.Providers.GetFilters(context.Controller.ControllerContext, context.ActionDescriptor).ToList();

            store.Add(GlimpseConstants.Filters, filters);
        }
    }
}