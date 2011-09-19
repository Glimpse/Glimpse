using System;
using System.Diagnostics;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseResultFilter : GlimpseFilter, IResultFilter
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
            var metadata = LogCall(OnResultExecutingGuid);
            var watch = new Stopwatch();
            watch.Start();

            using (GlimpseTimer.Start("IResultFilter OnResultExecuting" + ResultFilter.GetType().Name, "MVC"))
            {
                ResultFilter.OnResultExecuting(filterContext);
            }

            watch.Stop();
            metadata.ExecutionTime = watch.Elapsed;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var metadata = LogCall(OnResultExecutedGuid);
            var watch = new Stopwatch();
            watch.Start();

            using (GlimpseTimer.Start("IResultFilter OnResultExecuted:" + ResultFilter.GetType().Name, "MVC"))
            {
                ResultFilter.OnResultExecuted(filterContext);
            }

            watch.Stop();

            metadata.ExecutionTime = watch.Elapsed;
        }
    }
}
