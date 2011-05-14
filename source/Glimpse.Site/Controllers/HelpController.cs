using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class HelpController : Controller
    {
        //
        // GET: /Help/

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Plugin(string name)
        {
            return View();
        }
    }
}
