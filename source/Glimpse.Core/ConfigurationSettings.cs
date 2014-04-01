using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core
{
    /// <summary>
    /// Contains all the settings necessary to create a <see cref="Configuration" />. This abstraction allows any <see cref="IConfigurationSettingsProvider" /> 
    /// implementation to provide those settings, independing of where and how they are stored.
    /// </summary>
    public class ConfigurationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSettings" /> class
        /// </summary>
        /// <param name="resourceEndpointConfiguration">The resource endpoint configuration</param>
        /// <param name="persistenceStore">The persistence store</param>
        /// <param name="currentGlimpseRequestIdTracker">The current Glimpse request id tracker</param>
        /// <param name="defaultRuntimePolicy">The default runtime policy</param>
        /// <param name="endpointBaseUri">The endpoint base URI</param>
        /// <param name="loggingSettings">The logging settings</param>
        /// <param name="clientScriptsCollectionSettings">The client scripts collection settings</param>
        /// <param name="inspectorsCollectionSettings">The inspectors collection settings</param>
        /// <param name="resourcesCollectionSettings">The resources collection settings</param>
        /// <param name="runtimePoliciesCollectionSettings">The runtime policies collection settings</param>
        /// <param name="tabsCollectionSettings">The tabs collection settings</param>
        /// <param name="serializationConvertersCollectionSettings">The serialization converters collection settings</param>
        /// <param name="metadataCollectionSettings">The metadata collection settings</param>
        /// <param name="tabMetadataCollectionSettings">The tab metadata collection settings</param>
        /// <param name="displaysCollectionSettings">The displays collection settings</param>
        /// <param name="instanceMetadataCollectionSettings">The instance metadata collection settings</param>
        public ConfigurationSettings(
            IResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker,
            RuntimePolicy defaultRuntimePolicy,
            string endpointBaseUri,
            LoggingSettings loggingSettings,
            CollectionSettings clientScriptsCollectionSettings,
            CollectionSettings inspectorsCollectionSettings,
            CollectionSettings resourcesCollectionSettings,
            CollectionSettings runtimePoliciesCollectionSettings,
            CollectionSettings tabsCollectionSettings,
            CollectionSettings serializationConvertersCollectionSettings,
            CollectionSettings metadataCollectionSettings,
            CollectionSettings tabMetadataCollectionSettings,
            CollectionSettings displaysCollectionSettings,
            CollectionSettings instanceMetadataCollectionSettings)
        {
            Guard.ArgumentNotNull("resourceEndpointConfiguration", resourceEndpointConfiguration);
            Guard.ArgumentNotNull("persistenceStore", persistenceStore);
            Guard.ArgumentNotNull("currentGlimpseRequestIdTracker", currentGlimpseRequestIdTracker);
            Guard.StringNotNullOrEmpty("endpointBaseUri", endpointBaseUri);
            Guard.ArgumentNotNull("LoggingSettings", loggingSettings);
            Guard.ArgumentNotNull("clientScriptsCollectionSettings", clientScriptsCollectionSettings);
            Guard.ArgumentNotNull("inspectorsCollectionSettings", inspectorsCollectionSettings);
            Guard.ArgumentNotNull("resourcesCollectionSettings", resourcesCollectionSettings);
            Guard.ArgumentNotNull("resourcesCollectionSettings", resourcesCollectionSettings);
            Guard.ArgumentNotNull("runtimePoliciesCollectionSettings", runtimePoliciesCollectionSettings);
            Guard.ArgumentNotNull("tabsCollectionSettings", tabsCollectionSettings);
            Guard.ArgumentNotNull("serializationConvertersCollectionSettings", serializationConvertersCollectionSettings);
            Guard.ArgumentNotNull("metadataCollectionSettings", metadataCollectionSettings);
            Guard.ArgumentNotNull("tabMetadataCollectionSettings", tabMetadataCollectionSettings);
            Guard.ArgumentNotNull("displaysCollectionSettings", displaysCollectionSettings);
            Guard.ArgumentNotNull("instanceMetadataCollectionSettings", instanceMetadataCollectionSettings);

            ResourceEndpointConfiguration = resourceEndpointConfiguration;
            PersistenceStore = persistenceStore;
            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker;
            DefaultRuntimePolicy = defaultRuntimePolicy;
            EndpointBaseUri = endpointBaseUri;
            LoggingSettings = loggingSettings;
            ClientScriptsCollectionSettings = clientScriptsCollectionSettings;
            InspectorsCollectionSettings = inspectorsCollectionSettings;
            ResourcesCollectionSettings = resourcesCollectionSettings;
            RuntimePoliciesCollectionSettings = runtimePoliciesCollectionSettings;
            TabsCollectionSettings = tabsCollectionSettings;
            SerializationConvertersCollectionSettings = serializationConvertersCollectionSettings;
            MetadataCollectionSettings = metadataCollectionSettings;
            TabMetadataCollectionSettings = tabMetadataCollectionSettings;
            DisplaysCollectionSettings = displaysCollectionSettings;
            InstanceMetadataCollectionSettings = instanceMetadataCollectionSettings;
        }

        /// <summary>
        /// Gets the resource endpoint configuration
        /// </summary>
        public IResourceEndpointConfiguration ResourceEndpointConfiguration { get; private set; }

        /// <summary>
        /// Gets the persistence store
        /// </summary>
        public IPersistenceStore PersistenceStore { get; private set; }

        /// <summary>
        /// Gets the current Glimpse request id tracker
        /// </summary>
        public ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; private set; }

        /// <summary>
        /// Gets the default runtime policy
        /// </summary>
        public RuntimePolicy DefaultRuntimePolicy { get; private set; }

        /// <summary>
        /// Gets the endpoint base URI
        /// </summary>
        public string EndpointBaseUri { get; private set; }

        /// <summary>
        /// Gets the logging settings
        /// </summary>
        public LoggingSettings LoggingSettings { get; private set; }

        /// <summary>
        /// Gets the client scripts collection settings
        /// </summary>
        public CollectionSettings ClientScriptsCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the inspectors collection settings
        /// </summary>
        public CollectionSettings InspectorsCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the resources collection settings
        /// </summary>
        public CollectionSettings ResourcesCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the runtime policies collection settings
        /// </summary>
        public CollectionSettings RuntimePoliciesCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the tabs collection settings
        /// </summary>
        public CollectionSettings TabsCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the serialization converters collection settings
        /// </summary>
        public CollectionSettings SerializationConvertersCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the metadata collection settings
        /// </summary>
        public CollectionSettings MetadataCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the tab metadata collection settings
        /// </summary>
        public CollectionSettings TabMetadataCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the display collection settings
        /// </summary>
        public CollectionSettings DisplaysCollectionSettings { get; private set; }

        /// <summary>
        /// Gets the instance metadata collection settings
        /// </summary>
        public CollectionSettings InstanceMetadataCollectionSettings { get; private set; }
    }
}