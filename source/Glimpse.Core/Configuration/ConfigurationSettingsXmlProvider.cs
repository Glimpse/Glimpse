using System;
using System.Linq;
using System.Xml;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Implementation of an <see cref="IConfigurationSettingsProvider" /> that provides <see cref="ConfigurationSettings" /> based on a <see cref="XmlDocument"/> instance
    /// </summary>
    internal class ConfigurationSettingsXmlProvider : IConfigurationSettingsProvider
    {
        private XmlDocument ConfigurationDocument { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSettingsXmlProvider" /> class.
        /// </summary>
        /// <param name="configurationDocument">The configuration document</param>
        public ConfigurationSettingsXmlProvider(XmlDocument configurationDocument)
        {
            Guard.ArgumentNotNull(configurationDocument, "configurationDocument");
            ConfigurationDocument = configurationDocument;
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

            var collectionSettingsFactory = new CollectionSettingsFactory(ConfigurationDocument.GetAttributeValue("discoveryLocation", false, string.Empty));

            return new ConfigurationSettings(
               resourceEndpointConfiguration,
               persistenceStore,
               currentGlimpseRequestIdTracker,
               ConfigurationDocument.GetAttributeValueAsEnum("defaultRuntimePolicy", false, RuntimePolicy.On),
               ConfigurationDocument.GetAttributeValue("endpointBaseUri", false, "~/Glimpse.axd"),
               CreateLoggingSettings(),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("clientScripts")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("inspectors")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("resources")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("runtimePolicies")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("tabs")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("serializationConverters")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("metadata")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("tabMetadata")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("displays")),
               collectionSettingsFactory.Create(GetXmlElementOrDefault("instanceMetadata")));
        }

        private LoggingSettings CreateLoggingSettings()
        {
            var loggingElement = GetXmlElementOrDefault("logging");
            return loggingElement != null
                            ? new LoggingSettings(
                                loggingElement.GetAttributeValueAsEnum("level", false, LoggingLevel.Trace),
                                loggingElement.GetAttributeValue("logLocation", false, string.Empty))
                            : new LoggingSettings(LoggingLevel.Trace, string.Empty);

        }

        private XmlElement GetXmlElementOrDefault(string elementName)
        {
            try
            {
                return ConfigurationDocument.GetElementsByTagName(elementName).OfType<XmlElement>().SingleOrDefault();
            }
            catch (Exception)
            {
#warning log exception
                return null;
            }
        }
    }
}