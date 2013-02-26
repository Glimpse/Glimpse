using System;
using System.Web.Mvc;

namespace Glimpse.Test.Integration.Site.Controllers
{
    public class AsyncTestController : AsyncController
    {
        public void IndexAsync(string id)
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, id);

            AsyncManager.OutstandingOperations.Increment();

            var waitService = new WaitService();
            waitService.GetHeadlinesCompleted += result =>
            {
                AsyncManager.Parameters["id"] = result;
                AsyncManager.OutstandingOperations.Decrement();
            };
            waitService.GetHeadlinesAsync(id);
        }

        public ActionResult IndexCompleted(string id)
        {
            return View();
        }

    }

    public class WaitService
    {
        public event Action<string> GetHeadlinesCompleted;

        public void GetHeadlinesAsync(string id)
        {
            GetHeadlinesCompleted(id);
        }
    }
}
