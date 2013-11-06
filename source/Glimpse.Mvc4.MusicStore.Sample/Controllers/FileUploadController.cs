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
                byte[] input = new byte[file.ContentLength];
                file.InputStream.Read(input, 0, file.ContentLength);

                return RedirectToAction("Index", new { fileName = file.FileName });
            }
            catch
            {
                return View();
            }
        }
    }
}
