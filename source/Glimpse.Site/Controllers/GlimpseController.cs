using System.Configuration;
using System.Web.Mvc;
using Glimpse.Site.Framework;

namespace Glimpse.Site.Controllers
{
    public partial class GlimpseController : Controller
    {
        public virtual ActionResult CurrentVersion()
        {
            var setting = ConfigurationManager.AppSettings["CurrentGlimpseVersion"];
            var currentGlimpseVersion = decimal.Parse(setting);

            return new JsonpResult {Data = currentGlimpseVersion};
        }
    }
}
