using System.Web.Mvc;

namespace Glimpse.Test.Integration.Site.Areas.Area.Controllers
{
    public class HomeAreaController : Controller
    {
        public ActionResult Index()
        {
            var areaName = ControllerContext.RouteData.DataTokens["area"].ToString();
            HttpContext.Items.Add(IntegrationTestTab.Expected, areaName);

            return View(model: areaName);
        }
    }
}
