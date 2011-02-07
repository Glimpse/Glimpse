using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Glimpse.Net.Configuration;

namespace Glimpse.Test.Controllers
{
    public class MainController : Controller
    {
        [OutputCache(Duration = 1)]
        public ActionResult Index(int? id)
        {
            ViewData["viewData"] = "controller set viewdata";
            TempData["tempData"] = "controller set tempdata";
            ViewBag.ViewBagData = "controller set viewbag";
            Session["SessionString"] = "controller set session";
            Session["SessionInt"] = 3;
            Session["SessionDate"] = DateTime.Now;
            Session["SessionComplex"] = new Dictionary<string, string> { { "prop1", "val1" }, { "prop2", "val2" }}; 


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
