using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseExceptionFilter : GlimpseFilter, IExceptionFilter
    {
        public IExceptionFilter ExceptionFilter { get; set; }

        public GlimpseExceptionFilter(IExceptionFilter exceptionFilter)
        {
            ExceptionFilter = exceptionFilter;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var watch = new Stopwatch();
            watch.Start();

            ExceptionFilter.OnException(filterContext);

            watch.Stop();
            LogCall(Guid, watch.Elapsed);
        }
    }
}
