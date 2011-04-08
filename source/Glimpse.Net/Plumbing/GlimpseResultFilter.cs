using System;
using System.Collections.Generic;
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
            LogCall(OnResultExecutingGuid);

            ResultFilter.OnResultExecuting(filterContext);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (!HasExecuted)
            {
                HasExecuted = true;
                //TODO: FIX THIS HACK
                var metadata = GlimpseFilterCallMetadata.ActionResult(filterContext.Result);
                var store = filterContext.HttpContext.Items[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;

                store.Add(metadata);
                LogCall(metadata.Guid);
            }

            LogCall(OnResultExecutedGuid);

            ResultFilter.OnResultExecuted(filterContext);
        }
    }
}
