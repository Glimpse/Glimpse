using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Configuration
{
    [GlimpsePlugin]
    public class ConnectionStrings:IGlimpsePlugin
    {
        public string Name
        {
            get { return "ConnectionStrings"; }
        }

        public object GetData(HttpApplication application)
        {
            var connStringsData = new Dictionary<string, string>();
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                connStringsData.Add(item.Name, item.ConnectionString);
            }

            if (connStringsData.Count == 0) return null;

            return connStringsData;
        }
    }
}
