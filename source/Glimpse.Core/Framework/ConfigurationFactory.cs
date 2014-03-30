using System.Configuration;
using Glimpse.Core.Configuration;

namespace Glimpse.Core.Framework
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Create(
            ResourceEndpointConfiguration endpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return Create(endpointConfiguration, persistenceStore, "glimpse", currentGlimpseRequestIdTracker);
        }

        public static IConfiguration Create(
            ResourceEndpointConfiguration endpointConfiguration,
            IPersistenceStore persistenceStore,
            string xmlConfigurationSectionName,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return Create(endpointConfiguration, persistenceStore, ConfigurationManager.GetSection(xmlConfigurationSectionName) as Section, currentGlimpseRequestIdTracker);
        }

        public static IConfiguration Create(
            ResourceEndpointConfiguration endpointConfiguration,
            IPersistenceStore persistenceStore,
            Section xmlConfigurationSection,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            var configurationSettings = new ConfigurationSettings(
                endpointConfiguration,
                persistenceStore,
                currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker(),
                xmlConfigurationSection.DefaultRuntimePolicy,
                xmlConfigurationSection.EndpointBaseUri,
                new ConfigurationLoggingSettings(xmlConfigurationSection.Logging.Level, xmlConfigurationSection.Logging.LogLocation),
                new CollectionConfigurationFactory(xmlConfigurationSection.ClientScripts.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.Inspectors.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.Resources.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.RuntimePolicies.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.Tabs.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.SerializationConverters.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.Metadata.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.TabMetadata.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.Displays.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create(),
                new CollectionConfigurationFactory(xmlConfigurationSection.InstanceMetadata.XmlContent, xmlConfigurationSection.DiscoveryLocation).Create());

            return new Configuration(configurationSettings);
        }
    }
}