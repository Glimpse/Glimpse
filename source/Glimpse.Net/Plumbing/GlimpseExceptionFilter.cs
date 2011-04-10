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
            Debug.Write(string.Format("{0} for {1} result, {2} controller which is child:{3}", "OnException", filterContext.Result.GetType().Name, filterContext.Controller.GetType().Name, filterContext.IsChildAction));
            LogCall(Guid);

            ExceptionFilter.OnException(filterContext);
        }
    }
}
