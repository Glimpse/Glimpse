using System;

namespace Glimpse.Core.Configuration
{
    public class CustomConfigurationElement
    {
        public string Key { get; set; }
        public Type Type { get; set; }
        public string ConfigurationContent { get; set; }
    }
}
