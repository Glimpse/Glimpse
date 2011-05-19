using System.Web.Mvc;

namespace MvcMusicStore.Areas.Test
{
    public class TestAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Test";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Test_default",
                "Test/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Test_not used",
                "Test/{controller}/NeverUsed/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
