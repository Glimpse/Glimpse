using System.Collections.Generic;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class ConfigurationRoleManagerProviderSettingsModelConverter : SerializationConverter<List<ConfigurationRoleManagerProviderSettingsModel>>
    {
        public override object Convert(List<ConfigurationRoleManagerProviderSettingsModel> obj)
        {
            var root = new TabSection("Name", "Type", "Parameters");
            foreach (var item in obj)
            {
                root.AddRow().Column(item.Name).Column(item.Type).Column(item.Parameters);
            }

            return root.Build();
        }
    }
}