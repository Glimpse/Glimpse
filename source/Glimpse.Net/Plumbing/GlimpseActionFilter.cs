using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var watch = new Stopwatch();
            watch.Start();

            ActionFilter.OnActionExecuting(filterContext);

            watch.Stop();

            LogCall(OnActionExecutingGuid, watch.Elapsed);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var watch = new Stopwatch();
            watch.Start();

            ActionFilter.OnActionExecuted(filterContext);

            watch.Start();

            LogCall(OnActionExecutedGuid, watch.Elapsed);
        }
    }
}