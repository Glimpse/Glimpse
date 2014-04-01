using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents a <see cref="ConfigurationSettings" /> provider
    /// </summary>
    public interface IConfigurationSettingsProvider
    {
        /// <summary>
        /// Returns <see cref="ConfigurationSettings" /> based on the given input and the implementation of the provider
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="currentGlimpseRequestIdTracker">The current Glimpse request Id tracker</param>
        /// <returns>The <see cref="ConfigurationSettings"/></returns>
        ConfigurationSettings GetConfigurationSettings(
           ResourceEndpointConfiguration resourceEndpointConfiguration,
           IPersistenceStore persistenceStore,
           ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker);
    }
}