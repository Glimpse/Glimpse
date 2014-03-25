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
            return new Configuration(endpointConfiguration, persistenceStore, "glimpse", currentGlimpseRequestIdTracker);
        }

        public static IConfiguration Create(
            ResourceEndpointConfiguration endpointConfiguration,
            IPersistenceStore persistenceStore,
            string xmlConfigurationSectionName,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return new Configuration(endpointConfiguration, persistenceStore, ConfigurationManager.GetSection(xmlConfigurationSectionName) as Section, currentGlimpseRequestIdTracker);
        }

        public static IConfiguration Create(
            ResourceEndpointConfiguration endpointConfiguration,
            IPersistenceStore persistenceStore,
            Section xmlConfigurationSection,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return new Configuration(endpointConfiguration, persistenceStore, xmlConfigurationSection, currentGlimpseRequestIdTracker);
        }
    }
}
