using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class FileUploadController : Controller
    {
        public ActionResult Index(string fileName = "")
        {
            ViewBag.FileName = fileName;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index", new { fileName = file.FileName });
            }
            catch
            {
                return View();
            }
        }
    }
}
