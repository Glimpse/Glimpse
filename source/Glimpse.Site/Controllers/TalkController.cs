using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class TalkController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        } 
    }
}