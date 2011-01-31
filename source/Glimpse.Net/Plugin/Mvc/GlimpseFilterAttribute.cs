using System.Web.Mvc;

namespace Glimpse.Net.Mvc
{
    public class GlimpseFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var contextStore = filterContext.HttpContext.Items;

            contextStore.Add(GlimpseConstants.TempData, filterContext.Controller.TempData);
            contextStore.Add(GlimpseConstants.ViewData, filterContext.Controller.ViewData);
        }
    }
}