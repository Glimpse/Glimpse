using System.Web.Mvc;

namespace Glimpse.Test.Integration.Site.Controllers
{
    public class RoutingController : Controller
    {
        public ActionResult Subdomain()
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, RouteData.Values["subdomain"]);

            return View();
        }
    }
}
