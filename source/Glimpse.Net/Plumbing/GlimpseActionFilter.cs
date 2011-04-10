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

        public GlimpseActionFilter(IActionFilter actionFilter)
        {
            ActionFilter = actionFilter;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogCall(OnActionExecutingGuid);

            ActionFilter.OnActionExecuting(filterContext);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            LogCall(OnActionExecutedGuid);

            ActionFilter.OnActionExecuted(filterContext);
        }
    }
}