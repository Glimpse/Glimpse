using System.Configuration;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Factory for <see cref="IConfiguration" /> instances
    /// </summary>
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Creates an <see cref="IConfiguration" /> based on a <see cref="Section" /> resolved with the "glimpse" configuration section name
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="currentGlimpseRequestIdTracker">An optional <see cref="ICurrentGlimpseRequestIdTracker"/> which will default to <see cref="CallContextCurrentGlimpseRequestIdTracker"/></param>
        /// <returns>The <see cref="IConfiguration"/></returns>
        public static IConfiguration Create(
            ResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return Create(resourceEndpointConfiguration, persistenceStore, ConfigurationManager.GetSection("glimpse") as Section, currentGlimpseRequestIdTracker);
        }

        /// <summary>
        /// Creates an <see cref="IConfiguration" /> based on a <see cref="Section" /> resolved with the <paramref name="xmlConfigurationSectionName"/> configuration section name
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="xmlConfigurationSectionName">The configuration section name</param>
        /// <param name="currentGlimpseRequestIdTracker">An optional <see cref="ICurrentGlimpseRequestIdTracker"/> which will default to <see cref="CallContextCurrentGlimpseRequestIdTracker"/></param>
        /// <returns>The <see cref="IConfiguration"/></returns>
        public static IConfiguration Create(
            ResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            string xmlConfigurationSectionName,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            return Create(resourceEndpointConfiguration, persistenceStore, ConfigurationManager.GetSection(xmlConfigurationSectionName) as Section, currentGlimpseRequestIdTracker);
        }

        /// <summary>
        /// Creates an <see cref="IConfiguration" /> based on the given <see cref="Section" />
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="xmlConfigurationSection">The configuration section</param>
        /// <param name="currentGlimpseRequestIdTracker">An optional <see cref="ICurrentGlimpseRequestIdTracker"/> which will default to <see cref="CallContextCurrentGlimpseRequestIdTracker"/></param>
        /// <returns>The <see cref="IConfiguration"/></returns>
        public static IConfiguration Create(
            ResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            Section xmlConfigurationSection,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
#warning based on the Section we could allow another IConfigurationSettingsProvider to be set, so that the complete configuration could be loaded from somewhere else... (DB, File, ...)

            return new Configuration(
                new ConfigurationSettingsXmlProvider(xmlConfigurationSection).GetConfigurationSettings(
                    resourceEndpointConfiguration,
                    persistenceStore,
                    currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker()));
        }
    }
}