using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseResultFilter : GlimpseFilter, IResultFilter
    {
        public IResultFilter ResultFilter { get; set; }
        public Guid OnResultExecutingGuid { get; set; }
        public Guid OnResultExecutedGuid { get; set; }
        private static bool HasExecuted { get; set; }

        public GlimpseResultFilter(IResultFilter resultFilter)
        {
            ResultFilter = resultFilter;
            HasExecuted = false;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Debug.Write(string.Format("{0} for {1} is child:{2}", "OnResultExecuting", filterContext.Result.GetType().Name, filterContext.IsChildAction));
            LogCall(OnResultExecutingGuid);

            ResultFilter.OnResultExecuting(filterContext);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
/*            if (!HasExecuted)
            {
                HasExecuted = true;
                //TODO: FIX THIS HACK
                var metadata = GlimpseFilterCallMetadata.ActionResult(filterContext.Result);
                var store = filterContext.HttpContext.Items[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;

                store.Add(metadata);
                Debug.Write(string.Format("HAS EXECUTED! {0} for {1} result, {2} controller which is child:{3}", "OnResultExecuted", filterContext.Result.GetType().Name, filterContext.Controller.GetType().Name, filterContext.IsChildAction));
                LogCall(metadata.Guid);
            }*/

            Debug.Write(string.Format("{0} for {1} result, {2} controller which is child:{3}", "OnResultExecuted", filterContext.Result.GetType().Name, filterContext.Controller.GetType().Name, filterContext.IsChildAction));
            LogCall(OnResultExecutedGuid);

            ResultFilter.OnResultExecuted(filterContext);
        }
    }
}
