using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Glimpse.Test
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var test = routes.MapRoute("Test", "Test/", new {controller = "Test", action = "Non"});
            test.DataTokens["Name"] = "Test";

            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("Garbage", "Garbage/{action}/{thingy}/", new {controller = "Garbage", action = "Garbage"});

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );


            routes.MapRoute("Weird", "Weird/{otherthing}", new { controller = "Weird", action = "Weird" }, new { otherthing = @"\d+" });

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}