using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class ModelBindingTestsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PostItem(ItemContainingDictionary item)
        {
            return new JsonResult { Data = new { PostedItem = item} };
        }

        public class ItemContainingDictionary
        {
            public Dictionary<string, string> Stuff { get; set; }
        }
    }
}
