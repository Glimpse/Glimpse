using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    public class ConfigurationModel
    {
        public IEnumerable<ConfigurationConnectionStringModel> ConnectionStrings { get; set; }

        public IDictionary<string, string> AppSettings { get; set; }

        public ConfigurationAuthenticationModel Authentication { get; set; }

        public ConfigurationCustomErrorsModel CustomErrors { get; set; }

        public IEnumerable<ConfigurationHttpModulesModel> HttpModules { get; set; }

        public IEnumerable<ConfigurationHttpHandlersModel> HttpHandlers { get; set; }

        public ConfigurationRoleManagerModel RoleManager { get; set; }
    }
}
