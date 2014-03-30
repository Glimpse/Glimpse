using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    public interface IConfigurationSettingsProvider
    {
        ConfigurationSettings GetConfigurationSettings(
           ResourceEndpointConfiguration resourceEndpointConfiguration,
           IPersistenceStore persistenceStore,
           ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker);
    }
}