using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Test.Integration.Site.Code;

namespace Glimpse.Test.Integration.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("DomainRoute", new DomainRoute(
                "{subdomain}.localtest.me",     // Domain with parameters    
                "{action}/{id}",    // URL with parameters
                new { controller = "Routing", action = "Subdomain", id = string.Empty }  
            ));

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinderProviders.BinderProviders.Add(new CustomModelBinderProvider());
        }
    }
}