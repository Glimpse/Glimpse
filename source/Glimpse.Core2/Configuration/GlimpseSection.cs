using System;
using System.ComponentModel;
using System.Configuration;

namespace Glimpse.Core2.Configuration
{
    public class GlimpseSection : ConfigurationSection
    {
        [ConfigurationProperty("logging")]
        public LoggingElement Logging
        {
            get { return (LoggingElement) base["logging"]; }
            set { base["logging"] = value; }
        }

        [ConfigurationProperty("clientScripts")]
        public DiscoverableCollectionElement ClientScripts
        {
            get { return (DiscoverableCollectionElement) base["clientScripts"]; }
            set { base["clientScripts"] = value; }
        }
    }

    public class DiscoverableCollectionElement : ConfigurationElement
    {
        internal const string DefaultLocation = "";

        [ConfigurationProperty("autoDiscover", DefaultValue = true)]
        public bool AutoDiscover
        {
            get { return (bool) base["autoDiscover"]; }
            set { base["autoDiscover"] = value; }
        }

        [ConfigurationProperty("discoveryLocation", DefaultValue = DefaultLocation)]
        public string DiscoveryLocation
        {
            get { return (string) base["discoveryLocation"]; }
            set { base["discoveryLocation"] = value; }
        }

        [ConfigurationProperty("ignoredTypes")]
        public TypeElementCollection IgnoredTypes
        {
            get { return (TypeElementCollection) base["ignoredTypes"]; }
            set { base["ignoredTypes"] = value; }
        }
    }

    [ConfigurationCollection(typeof (TypeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class TypeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TypeElement) element).Type;
        }
    }

    public class TypeElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        [TypeConverter(typeof(TypeConverter))]
        public Type Type
        {
            get { return (Type) base["type"]; }
            set { base["type"] = value; }
        }
    }
}