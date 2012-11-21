using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.SerializationConverter;
using Xunit;

namespace Glimpse.Test.AspNet.SerializationConverter
{
    public class ConfigurationRoleManagerProviderSettingsModelConverterShould
    {
        [Fact]
        public void ConvertToList()
        {
            var model = new List<ConfigurationRoleManagerProviderSettingsModel> { new ConfigurationRoleManagerProviderSettingsModel { Name = "Name Test", Type = "Type Test", Parameters = new Dictionary<string, string> { { "Test", "Pest" }, { "Jester", "Wester" } } } };
             
            var converter = new ConfigurationRoleManagerProviderSettingsModelConverter();
            var result = converter.Convert(model) as IEnumerable<object>;

            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }
    }
}
