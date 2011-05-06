using System.Web.Mvc;

namespace Glimpse.Site.Controllers
{
    public partial class QuestionsController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        } 
    }
}