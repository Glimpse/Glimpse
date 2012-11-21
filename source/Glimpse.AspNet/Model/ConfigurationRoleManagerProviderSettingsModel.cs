using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.AspNet.Model
{
    public class ConfigurationRoleManagerProviderSettingsModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public IDictionary<string, string> Parameters { get; set; }
    }
}
