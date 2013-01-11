using System.Web.Mvc;

namespace Glimpse.Test.Integration.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, id);

            return View();
        }
    }
}
