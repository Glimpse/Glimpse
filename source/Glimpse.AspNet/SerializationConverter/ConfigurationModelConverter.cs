using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.SerializationConverter
{ 
    public class ConfigurationModelConverter : SerializationConverter<ConfigurationModel>
    {
        public override object Convert(ConfigurationModel obj)
        {
            var root = new TabSection("Key", "Value");
            root.AddRow().Column("/configuration/appSettings").Column(obj.AppSettings);
            root.AddRow().Column("/configuration/connectionStrings").Column(BuildConnectionStringsDetails(obj.ConnectionStrings));
            root.AddRow().Column("/configuration/system.web/authentication").Column(obj.Authentication);
            root.AddRow().Column("/configuration/system.web/roleManager").Column(obj.RoleManager);
            root.AddRow().Column("/configuration/system.web/customErrors").Column(obj.CustomErrors);
            root.AddRow().Column("/configuration/system.web/httpModules").Column(obj.HttpModules);
            root.AddRow().Column("/configuration/system.web/httpHandlers").Column(obj.HttpHandlers);

            return root.Build();
        }

        private TabSection BuildConnectionStringsDetails(IEnumerable<ConfigurationConnectionStringModel> model)
        {
            var section = new TabSection("Key", "ProviderName", "Connection String");
            foreach (var item in model)
            {
                section.AddRow().Column(item.Key).Column(item.ProviderName).Column(new { item.Raw, item.Details });    
            }

            return section;
        }
    } 
}
