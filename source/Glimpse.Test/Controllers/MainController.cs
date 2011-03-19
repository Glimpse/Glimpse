using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net;
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
            Session["SessionBigInt"] = int.MaxValue;
            Session["SessionDate"] = DateTime.Now;
            Session["SessionComplex"] = new Dictionary<string, string> { { "prop1", "val1" }, { "prop2", "val2" }};

            Trace.Assert(true, "Assert w true", "detailMessage");
            //Trace.Assert(false, "Assert w false", "detailMessage");
            //Trace.Fail("Fail message", "detailMessage");
            Trace.TraceError("Trace Error {0}, {1}", 0, 1);
            Trace.TraceInformation("Trace Info, no format");
            Trace.TraceWarning("Trace Warning");
            Trace.WriteIf(true, "Writeif w true", "SomeCat");
            Trace.WriteIf(false, "Writeif w false", "SomeCat");

            Trace.WriteLine("Tracing test!");
            Trace.TraceError("This is an error");

            Debug.Write("This is from debug");
            
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
