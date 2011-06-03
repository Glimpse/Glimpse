using System.Web.Mvc;

namespace MvcMusicStore.Areas.Test.Controllers
{
    public partial class TestController : Controller
    {
        //
        // GET: /Test/Test/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
