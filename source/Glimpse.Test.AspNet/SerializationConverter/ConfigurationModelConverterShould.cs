using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.SerializationConverter;
using Xunit;

namespace Glimpse.Test.AspNet.SerializationConverter
{
    public class ConfigurationModelConverterShould
    {
        [Fact]
        public void ConvertToList()
        {
            var model = new ConfigurationModel();
            model.AppSettings = new Dictionary<string, string> { { "Test", "Pest" }, { "Jester", "Wester" } };
            model.Authentication = new ConfigurationAuthenticationModel();
            model.ConnectionStrings = new List<ConfigurationConnectionStringModel> { new ConfigurationConnectionStringModel(), new ConfigurationConnectionStringModel() };
            model.CustomErrors = new ConfigurationCustomErrorsModel();
            model.HttpHandlers = new List<ConfigurationHttpHandlersModel> { new ConfigurationHttpHandlersModel(), new ConfigurationHttpHandlersModel() };
            model.HttpModules = new List<ConfigurationHttpModulesModel> { new ConfigurationHttpModulesModel(), new ConfigurationHttpModulesModel() };
            model.RoleManager = new ConfigurationRoleManagerModel();

            var converter = new ConfigurationModelConverter();
            var result = converter.Convert(model) as IDictionary<object, object>;
             
            Assert.NotNull(result);
            Assert.True(result.Count > 0); 
        }
    }
}
