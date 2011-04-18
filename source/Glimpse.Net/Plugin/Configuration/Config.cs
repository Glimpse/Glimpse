using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Glimpse.Net.Extensibility;
using Glimpse.Net.Extentions;

namespace Glimpse.Net.Plugin.Configuration
{
    [GlimpsePlugin]
    internal class Config : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Config"; }
        }

        public object GetData(HttpApplication application)
        {
            //TODO, add in other useful config sections like compilation, 
            var connectionStrings = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().ToDictionary(item => item.Name, item => item.ConnectionString);
            var customErrorsSection = ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection;
            var authenticationSection = ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection;

            return new
                       {
                           AppSettings = ConfigurationManager.AppSettings.Flatten(),
                           ConnectionStrings = connectionStrings,
                           CustomErrors = customErrorsSection,
                           Authentication = authenticationSection
                       };
        }

        public void SetupInit(HttpApplication application)
        {
            throw new NotImplementedException();
        }
    }
}