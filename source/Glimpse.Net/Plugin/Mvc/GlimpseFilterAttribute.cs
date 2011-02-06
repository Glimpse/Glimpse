using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Glimpse.Net.Mvc
{
    public class GlimpseFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Add(GlimpseConstants.TempData, filterContext.Controller.TempData);
            store.Add(GlimpseConstants.ViewData, filterContext.Controller.ViewData);

            AddFilters(store, filterContext);
        }

        private static void AddFilters(IDictionary store, ActionExecutedContext context)
        {
            var filters = new List<Filter>();

            foreach (var provider in FilterProviders.Providers)
            foreach (var filter in provider.GetFilters(context.Controller.ControllerContext, context.ActionDescriptor))
            {
                filters.Add(filter);
            }

            store.Add(GlimpseConstants.Filters, filters);
        }
    }
}