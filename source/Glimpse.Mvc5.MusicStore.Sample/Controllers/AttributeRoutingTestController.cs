using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class AttributeRoutingTestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("attrRoute/Test", Name = "attrTest1")]
        [Route("attrRoute/Test/{id}", Name = "attrTest2")]
        [Route("attrRoute/{param}/Test/{id}", Name = "attrTest3")]
        public ActionResult AttrRouteTest()
        {
            return View();
        }
    }
}