using System;
using System.Web.Mvc;
using Glimpse.Test.Integration.Site.Models;

namespace Glimpse.Test.Integration.Site.Controllers
{
    public class BindingController : Controller
    {
        public ActionResult DefaultBinder(Guid id)
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, id.ToString());

            return View(model: id);
        }

        public ActionResult CustomBinder(CustomModel customModel)
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, customModel.ToString());

            return View(model: customModel);
        }
    }
}
