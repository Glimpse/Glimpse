using System.Configuration;
using System.Web.Mvc;
using Glimpse.Net.Configuration;

namespace Glimpse.Test.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            ViewData["viewData"] = "controller set viewdata";
            TempData["tempData"] = "controller set tempdata";
            ViewBag.ViewBagData = "controller set viewbag";
            Session["SessionData"] = "controller set session";

            var glimpseConfiguration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration;

            ViewBag.GlimpseOn = glimpseConfiguration.On;
            ViewBag.IpAddresses = glimpseConfiguration.IpAddresses;
            ViewBag.ContentTypes = glimpseConfiguration.ContentTypes;

            var cookie = Request.Cookies["glimpseMode"];

            if (cookie != null)
                ViewBag.GlimpseMode = cookie.Value;
            else
                ViewBag.GlimpseMode = "off";


            return View();
        }

    }
}
