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
            return new Configuration(
                DetermineConfigurationSettingsProvider().GetConfigurationSettings(
                    resourceEndpointConfiguration,
                    persistenceStore,
                    currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker()));
        }

        private static IConfigurationSettingsProvider DetermineConfigurationSettingsProvider()
        {
#warning based on the Section we could allow another IConfigurationSettingsProvider to be set, so that the complete configuration could be loaded from somewhere else... (DB, File, ...)
            return new ConfigurationSettingsXmlProvider(ConfigurationManager.GetSection("glimpse") as Section);
        }
    }
}