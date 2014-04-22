using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Implementation of an <see cref="IConfigurationSettingsProvider" /> that provides <see cref="ConfigurationSettings" /> based on a <see cref="Section"/> instance
    /// </summary>
    public class ConfigurationSettingsXmlProvider : IConfigurationSettingsProvider
    {
        private Section XmlConfiguration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSettingsXmlProvider" /> class.
        /// </summary>
        /// <param name="xmlConfiguration">The xml configuration</param>
        public ConfigurationSettingsXmlProvider(Section xmlConfiguration)
        {
            Guard.ArgumentNotNull(xmlConfiguration, "xmlConfiguration");
            XmlConfiguration = xmlConfiguration;
        }

        /// <summary>
        /// Returns <see cref="ConfigurationSettings" /> based on the given input and the <see cref="Section" /> information
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="currentGlimpseRequestIdTracker">The current Glimpse request Id tracker</param>
        /// <returns>The <see cref="ConfigurationSettings"/></returns>
        public ConfigurationSettings GetConfigurationSettings(
            ResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker)
        {
            Guard.ArgumentNotNull(resourceEndpointConfiguration, "resourceEndpointConfiguration");
            Guard.ArgumentNotNull(persistenceStore, "persistenceStore");
            Guard.ArgumentNotNull(currentGlimpseRequestIdTracker, "currentGlimpseRequestIdTracker");

            var collectionSettingsFactory = new CollectionSettingsFactory(XmlConfiguration.DiscoveryLocation);

            return new ConfigurationSettings(
               resourceEndpointConfiguration,
               persistenceStore,
               currentGlimpseRequestIdTracker,
               XmlConfiguration.DefaultRuntimePolicy,
               XmlConfiguration.EndpointBaseUri,
               new LoggingSettings(XmlConfiguration.Logging.Level, XmlConfiguration.Logging.LogLocation),
               collectionSettingsFactory.Create(XmlConfiguration.ClientScripts.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.Inspectors.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.Resources.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.RuntimePolicies.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.Tabs.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.SerializationConverters.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.Metadata.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.TabMetadata.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.Displays.XmlContent),
               collectionSettingsFactory.Create(XmlConfiguration.InstanceMetadata.XmlContent));
        }
    }
}