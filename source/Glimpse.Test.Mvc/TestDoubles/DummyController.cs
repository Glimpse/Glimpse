using System.Web.Mvc;

namespace Glimpse.Test.Mvc.TestDoubles
{
    public class DummyController : Controller
    {
         public ActionResult Index()
         {
             return null;
         }
    }
}