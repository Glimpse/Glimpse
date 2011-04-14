using System;
using System.Diagnostics;
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
            var watch = new Stopwatch();
            watch.Start();

            ResultFilter.OnResultExecuting(filterContext);

            watch.Stop();
            LogCall(OnResultExecutingGuid, watch.Elapsed);
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var watch = new Stopwatch();
            watch.Start();

            ResultFilter.OnResultExecuted(filterContext);

            watch.Stop();
            LogCall(OnResultExecutedGuid, watch.Elapsed);
        }
    }
}
