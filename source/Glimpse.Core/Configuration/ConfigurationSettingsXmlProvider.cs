using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    public class ConfigurationSettingsXmlProvider : IConfigurationSettingsProvider
    {
        private Section XmlConfiguration { get; set; }

        public ConfigurationSettingsXmlProvider(Section xmlConfiguration)
        {
            Guard.ArgumentNotNull("xmlConfiguration", xmlConfiguration);
            XmlConfiguration = xmlConfiguration;
        }

        public ConfigurationSettings GetConfigurationSettings(
            ResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker)
        {
            Guard.ArgumentNotNull("resourceEndpointConfiguration", resourceEndpointConfiguration);
            Guard.ArgumentNotNull("persistenceStore", persistenceStore);
            Guard.ArgumentNotNull("currentGlimpseRequestIdTracker", currentGlimpseRequestIdTracker);

            return new ConfigurationSettings(
               resourceEndpointConfiguration,
               persistenceStore,
               currentGlimpseRequestIdTracker,
               XmlConfiguration.DefaultRuntimePolicy,
               XmlConfiguration.EndpointBaseUri,
               new ConfigurationLoggingSettings(XmlConfiguration.Logging.Level, XmlConfiguration.Logging.LogLocation),
               new CollectionConfigurationFactory(XmlConfiguration.ClientScripts.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.Inspectors.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.Resources.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.RuntimePolicies.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.Tabs.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.SerializationConverters.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.Metadata.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.TabMetadata.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.Displays.XmlContent, XmlConfiguration.DiscoveryLocation).Create(),
               new CollectionConfigurationFactory(XmlConfiguration.InstanceMetadata.XmlContent, XmlConfiguration.DiscoveryLocation).Create());
        }
    }
}
