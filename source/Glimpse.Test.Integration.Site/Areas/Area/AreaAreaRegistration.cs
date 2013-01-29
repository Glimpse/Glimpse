using System.Web.Mvc;

namespace Glimpse.Test.Integration.Site.Areas.Area
{
    public class AreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Area";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Area_default",
                "Area/{controller}/{action}/{id}",
                new { controller = "HomeArea", action = "Index", id = UrlParameter.Optional });
        }
    }
}
