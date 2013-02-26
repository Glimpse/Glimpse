using System.Diagnostics;
using System.Threading;
using System.Web.Mvc;

namespace Glimpse.Test.Integration.Site.Controllers
{
    public class TraceController : Controller
    {
        public ActionResult TraceSource(string id)
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, id);

            var traceSource = new TraceSource("Test Source");
            traceSource.TraceEvent(TraceEventType.Warning, 0, id);

            return View(model: id);
        }

        public ActionResult ThreadTrace(string id)
        {
            HttpContext.Items.Add(IntegrationTestTab.Expected, id);

            var thread = new Thread(() => Trace.Write(id));
            thread.Start();

            return View(model: id);
        }
    }
}
