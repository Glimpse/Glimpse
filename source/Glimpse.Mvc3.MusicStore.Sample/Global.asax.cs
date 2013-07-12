using MvcMusicStore.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dapper;

namespace MvcMusicStore
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
            var myRoute = new Route("Blog/Archive/{year}/{month}/{day}", new RouteValueDictionary { { "controller", "Blog" }, { "action", "archive" }, { "year", "" }, { "month", "" },  { "day", "" }, { "page", 0 } }, null, new RouteValueDictionary { { "Test", "Data" }, { "Other", "Me" } }, new MvcRouteHandler());
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add(myRoute);

            routes.MapRoute(
                "BlogArchive",
                "Archive/{entryDate}",
                new { controller = "Blog", action = "Archive" },
                new { entryDate = @"d{2}-d{2}-d{4}" }
            );

            routes.MapRoute(
                "Product",
                "Product/Insert",
                new { controller = "Product", action = "Insert" },
                new { httpMethod = new HttpMethodConstraint("POST"), random = "Test" }
            );

            routes.MapRoute(
                "OtherDefaultBrowse", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Browse", id = UrlParameter.Optional }, // Parameter defaults
                new { controller = "Home", action = @"Test" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "DefaultTest", // Route name
                "{controller}/{action}/{id}/{test}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, test = 1 }, // Parameter defaults
                new { action = @"Index", controller = "Test" }
            ); 
        }

        private void LoadConfiguration()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MusicStoreEntities"];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString.ConnectionString;
                connection.Open();

                MvcMusicStore.Framework.Configuration.Current = connection.Query<ConfigurationModel>("SELECT * FROM Configuration").First(); 
            }
        }

        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new MvcMusicStore.Models.SampleData());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            LoadConfiguration();
        }
    }
}