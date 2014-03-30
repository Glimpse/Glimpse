using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core
{
    public class ConfigurationSettings
    {
        public ConfigurationSettings(
            IResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker,
            RuntimePolicy defaultRuntimePolicy,
            string endpointBaseUri,
            ConfigurationLoggingSettings configurationLoggingSettings,
            CollectionConfiguration clientScriptsConfiguration,
            CollectionConfiguration inspectorsConfiguration,
            CollectionConfiguration resourcesConfiguration,
            CollectionConfiguration runtimePoliciesConfiguration,
            CollectionConfiguration tabsConfiguration,
            CollectionConfiguration serializationConvertersConfiguration,
            CollectionConfiguration metadataConfiguration,
            CollectionConfiguration tabMetadataConfiguration,
            CollectionConfiguration displaysConfiguration,
            CollectionConfiguration instanceMetadataConfiguration)
        {
            Guard.ArgumentNotNull("resourceEndpointConfiguration", resourceEndpointConfiguration);
            Guard.ArgumentNotNull("persistenceStore", persistenceStore);
            Guard.ArgumentNotNull("currentGlimpseRequestIdTracker", currentGlimpseRequestIdTracker);
            Guard.StringNotNullOrEmpty("endpointBaseUri", endpointBaseUri);
            Guard.ArgumentNotNull("configurationLoggingSettings", configurationLoggingSettings);
            Guard.ArgumentNotNull("clientScripts", clientScriptsConfiguration);
            Guard.ArgumentNotNull("inspectorsConfiguration", inspectorsConfiguration);
            Guard.ArgumentNotNull("resourcesConfiguration", resourcesConfiguration);
            Guard.ArgumentNotNull("resourcesConfiguration", resourcesConfiguration);
            Guard.ArgumentNotNull("runtimePoliciesConfiguration", runtimePoliciesConfiguration);
            Guard.ArgumentNotNull("tabsConfiguration", tabsConfiguration);
            Guard.ArgumentNotNull("serializationConvertersConfiguration", serializationConvertersConfiguration);
            Guard.ArgumentNotNull("metadataConfiguration", metadataConfiguration);
            Guard.ArgumentNotNull("tabMetadataConfiguration", tabMetadataConfiguration);
            Guard.ArgumentNotNull("displaysConfiguration", displaysConfiguration);
            Guard.ArgumentNotNull("instanceMetadataConfiguration", instanceMetadataConfiguration);

            ResourceEndpointConfiguration = resourceEndpointConfiguration;
            PersistenceStore = persistenceStore;
            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker;
            DefaultRuntimePolicy = defaultRuntimePolicy;
            EndpointBaseUri = endpointBaseUri;
            Logging = configurationLoggingSettings;
            ClientScriptsConfiguration = clientScriptsConfiguration;
            ClientScriptsConfiguration = clientScriptsConfiguration;
            InspectorsConfiguration = inspectorsConfiguration;
            ResourcesConfiguration = resourcesConfiguration;
            RuntimePoliciesConfiguration = runtimePoliciesConfiguration;
            TabsConfiguration = tabsConfiguration;
            SerializationConvertersConfiguration = serializationConvertersConfiguration;
            MetadataConfiguration = metadataConfiguration;
            TabMetadataConfiguration = tabMetadataConfiguration;
            DisplaysConfiguration = displaysConfiguration;
            InstanceMetadataConfiguration = instanceMetadataConfiguration;
        }

        public IResourceEndpointConfiguration ResourceEndpointConfiguration { get; private set; }

        public IPersistenceStore PersistenceStore { get; private set; }

        public ConfigurationLoggingSettings Logging { get; private set; }

        public ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; private set; }

        public RuntimePolicy DefaultRuntimePolicy { get; private set; }

        public string EndpointBaseUri { get; private set; }

        public CollectionConfiguration ClientScriptsConfiguration { get; private set; }
        public CollectionConfiguration InspectorsConfiguration { get; private set; }
        public CollectionConfiguration ResourcesConfiguration { get; private set; }
        public CollectionConfiguration RuntimePoliciesConfiguration { get; private set; }
        public CollectionConfiguration TabsConfiguration { get; private set; }
        public CollectionConfiguration SerializationConvertersConfiguration { get; private set; }
        public CollectionConfiguration MetadataConfiguration { get; private set; }
        public CollectionConfiguration TabMetadataConfiguration { get; private set; }
        public CollectionConfiguration DisplaysConfiguration { get; private set; }
        public CollectionConfiguration InstanceMetadataConfiguration { get; private set; }
    }
}