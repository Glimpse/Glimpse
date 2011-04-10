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
        private static bool HasExecuted { get; set; }

        public GlimpseActionFilter(IActionFilter actionFilter)
        {
            ActionFilter = actionFilter;
            HasExecuted = false;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.Write(string.Format("{0} for {1} controller which is child:{2}", "OnActionExecuting", filterContext.Controller.GetType().Name, filterContext.IsChildAction));
            LogCall(OnActionExecutingGuid);

            ActionFilter.OnActionExecuting(filterContext);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
/*            if (!HasExecuted)
            {
                HasExecuted = true;

                //TODO: FIX THIS HACK
                var metadata = GlimpseFilterCallMetadata.ControllerAction(filterContext.ActionDescriptor);
                var store = filterContext.HttpContext.Items[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;

                store.Add(metadata);
                Debug.Write(string.Format("HAS EXECUTED!! {0} for {1} result, {2} controller which is child:{3}", "OnActionExecuted", filterContext.Result.GetType().Name, filterContext.Controller.GetType().Name, filterContext.IsChildAction));
                LogCall(metadata.Guid);
            }*/
            Debug.Write(string.Format("{0} for {1} result, {2} controller which is child:{3}", "OnActionExecuted", filterContext.Result.GetType().Name, filterContext.Controller.GetType().Name, filterContext.IsChildAction));
            LogCall(OnActionExecutedGuid);

            ActionFilter.OnActionExecuted(filterContext);
        }
    }
}