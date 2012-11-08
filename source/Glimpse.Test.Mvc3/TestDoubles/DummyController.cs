using System.Web.Mvc;

namespace Glimpse.Test.Mvc3.TestDoubles
{
    public class DummyController:Controller
    {
         public ActionResult Index()
         {
             return null;
         }
    }
}