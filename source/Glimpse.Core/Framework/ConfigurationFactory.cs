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
#warning based on the Section we could allow another IConfigurationSettingsProvider to be set, so that the complete configuration could be loaded from somewhere else... (DB, File, ...)

            return new Configuration(
                new ConfigurationSettingsXmlProvider(xmlConfigurationSection).GetConfigurationSettings(
                    endpointConfiguration,
                    persistenceStore,
                    currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker()));
        }
    }
}