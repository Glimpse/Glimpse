using System;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseResultFilter : GlimpseFilter, IResultFilter
    {
        public IResultFilter ResultFilter { get; set; }
        public Guid OnResultExecutingGuid { get; set; }
        public Guid OnResultExecutedGuid { get; set; }

        public GlimpseResultFilter(IResultFilter resultFilter)
        {
            ResultFilter = resultFilter;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            LogCall(OnResultExecutingGuid);

            ResultFilter.OnResultExecuting(filterContext);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            LogCall(OnResultExecutedGuid);

            ResultFilter.OnResultExecuted(filterContext);
        }
    }
}
