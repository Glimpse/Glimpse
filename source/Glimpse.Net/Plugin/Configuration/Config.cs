using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Configuration
{
    [GlimpsePlugin]
    public class Config : NameValueCollectionPlugin
    {
        public override string Name
        {
            get { return "Config"; }
        }

        public override object GetData(HttpApplication application)
        {
            var ConnectionStrings = new Dictionary<string, string>();
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                ConnectionStrings.Add(item.Name, item.ConnectionString);
            }

            if (ConnectionStrings.Count == 0) return null;

            //TODO, add in other useful config sections like customErrors, authentication, compilation, 
            //var customErrorsSection = ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection;

            return new
                       {
                           AppSettings = Process(ConfigurationManager.AppSettings, application),
                           ConnectionStrings
                       };
        }
    }
}