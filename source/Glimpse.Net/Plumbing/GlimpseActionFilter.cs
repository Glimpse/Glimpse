using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseActionFilter : GlimpseFilter, IActionFilter
    {
        public IActionFilter ActionFilter { get; set; }
        public Guid OnActionExecutingGuid { get; set; }
        public Guid OnActionExecutedGuid { get; set; }
        private static bool HasExecuted { get; set; }

        public GlimpseActionFilter(IActionFilter actionFilter)
        {
            ActionFilter = actionFilter;
            HasExecuted = false;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogCall(OnActionExecutingGuid);

            ActionFilter.OnActionExecuting(filterContext);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!HasExecuted)
            {
                HasExecuted = true;

                //TODO: FIX THIS HACK
                var metadata = GlimpseFilterCallMetadata.ControllerAction(filterContext.ActionDescriptor);
                var store = filterContext.HttpContext.Items[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;

                store.Add(metadata);

                LogCall(metadata.Guid);
            }

            LogCall(OnActionExecutedGuid);

            ActionFilter.OnActionExecuted(filterContext);
        }
    }
}