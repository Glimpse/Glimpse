using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    public class ConfigurationConnectionStringModel
    {
        public string Key { get; set; }

        public string Raw { get; set; }

        public string ProviderName { get; set; }

        public IDictionary<string, object> Details { get; set; }
    }
}