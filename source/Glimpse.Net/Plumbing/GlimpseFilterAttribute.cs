using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseFilterAttribute: ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Add(GlimpseConstants.Result, filterContext.Result);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Add(GlimpseConstants.TempData, filterContext.Controller.TempData);
            store.Add(GlimpseConstants.ViewData, filterContext.Controller.ViewData);
            store.Add(GlimpseConstants.RequestContext, filterContext.RequestContext);
        }
    }
}