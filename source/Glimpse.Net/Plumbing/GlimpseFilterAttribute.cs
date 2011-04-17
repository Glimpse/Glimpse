/*using System.Web.Mvc;
using Glimpse.Net.Extentions;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseFilterAttribute: ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Save(GlimpseConstants.Result, filterContext.Result);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var store = filterContext.HttpContext.Items;

            store.Save(GlimpseConstants.TempData, filterContext.Controller.TempData);
            store.Save(GlimpseConstants.ViewData, filterContext.Controller.ViewData);
            store.Save(GlimpseConstants.RequestContext, filterContext.RequestContext);
        }
    }
}*/