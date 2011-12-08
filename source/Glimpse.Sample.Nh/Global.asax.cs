using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NerdDinner
{
	using System.IO;
	using NHibernate;
	using NHibernate.Cfg;

	public class MvcApplication : HttpApplication
	{
		public static ISession CurrentSession
		{
			get
			{
				return (ISession)HttpContext.Current.Items["CurrentSession"];
			}
			set
			{
				HttpContext.Current.Items["CurrentSession"] = value;
			}
		}

		private static readonly object sessionFactoryLock = new object();
		private static ISessionFactory sessionFactory;

		public static ISessionFactory SessionFactory
		{
			get
			{
				if(sessionFactory == null)
				{
					lock(sessionFactoryLock)
					{
						if(sessionFactory == null)
						{
							sessionFactory = new Configuration()
								.Configure(
									Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml")
								)
								.BuildSessionFactory();
						}
					}
				}
				return sessionFactory;
			}
		}

		public MvcApplication()
		{
			BeginRequest += (sender, args) => CurrentSession = SessionFactory.OpenSession();
			EndRequest += (sender, args) => CurrentSession.Dispose();
		}

		public void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
					"PrettyDetails",
					"{Id}",
						new { controller = "Dinners", action = "Details" },
						new { Id = @"\d+" }
			);

			routes.MapRoute(
					"UpcomingDinners",
					"Dinners/Page/{page}",
					new { controller = "Dinners", action = "Index" }
			);

			routes.MapRoute(
					"Default",                                              // Route name
					"{controller}/{action}/{id}",                           // URL with parameters
					new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);
		}

		void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
		    Trace.Write(SessionFactory);
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new MobileCapableWebFormViewEngine());
		}
	}
}