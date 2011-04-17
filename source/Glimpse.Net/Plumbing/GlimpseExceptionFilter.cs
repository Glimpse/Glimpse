using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseExceptionFilter : GlimpseFilter, IExceptionFilter
    {
        public IExceptionFilter ExceptionFilter { get; set; }
        public Guid Guid { get; set; }

        public GlimpseExceptionFilter(IExceptionFilter exceptionFilter)
        {
            ExceptionFilter = exceptionFilter;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var metadata = LogCall(Guid);
            var watch = new Stopwatch();
            watch.Start();

            ExceptionFilter.OnException(filterContext);

            watch.Stop();
            metadata.ExecutionTime = watch.Elapsed;
        }
    }
}
