using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class DiscoverableCollectionElement : ConfigurationElement
    {
        internal const string DefaultLocation = "";

        [ConfigurationProperty("autoDiscover", DefaultValue = true)]
        public bool AutoDiscover
        {
            get { return (bool)base["autoDiscover"]; }
            set { base["autoDiscover"] = value; }
        }

        [ConfigurationProperty("discoveryLocation", DefaultValue = DefaultLocation)]
        public string DiscoveryLocation
        {
            get { return (string)base["discoveryLocation"]; }
            set { base["discoveryLocation"] = value; }
        }

        [ConfigurationProperty("ignoredTypes")]
        public TypeElementCollection IgnoredTypes
        {
            get { return (TypeElementCollection)base["ignoredTypes"]; }
            set { base["ignoredTypes"] = value; }
        }
    }
}