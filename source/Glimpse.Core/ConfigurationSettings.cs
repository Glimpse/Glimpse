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
        /// <param name="clientScriptsSettings">The client scripts settings</param>
        /// <param name="inspectorsSettings">The inspectors settings</param>
        /// <param name="resourcesSettings">The resources settings</param>
        /// <param name="runtimePoliciesSettings">The runtime policies settings</param>
        /// <param name="tabsSettings">The tabs settings</param>
        /// <param name="serializationConvertersSettings">The serialization converters settings</param>
        /// <param name="metadataSettings">The metadata settings</param>
        /// <param name="tabMetadataSettings">The tab metadata settings</param>
        /// <param name="displaysSettings">The displays settings</param>
        /// <param name="instanceMetadataSettings">The instance metadata settings</param>
        public ConfigurationSettings(
            IResourceEndpointConfiguration resourceEndpointConfiguration,
            IPersistenceStore persistenceStore,
            ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker,
            RuntimePolicy defaultRuntimePolicy,
            string endpointBaseUri,
            LoggingSettings loggingSettings,
            CollectionSettings clientScriptsSettings,
            CollectionSettings inspectorsSettings,
            CollectionSettings resourcesSettings,
            CollectionSettings runtimePoliciesSettings,
            CollectionSettings tabsSettings,
            CollectionSettings serializationConvertersSettings,
            CollectionSettings metadataSettings,
            CollectionSettings tabMetadataSettings,
            CollectionSettings displaysSettings,
            CollectionSettings instanceMetadataSettings)
        {
            Guard.ArgumentNotNull("resourceEndpointConfiguration", resourceEndpointConfiguration);
            Guard.ArgumentNotNull("persistenceStore", persistenceStore);
            Guard.ArgumentNotNull("currentGlimpseRequestIdTracker", currentGlimpseRequestIdTracker);
            Guard.StringNotNullOrEmpty("endpointBaseUri", endpointBaseUri);
            Guard.ArgumentNotNull("LoggingSettings", loggingSettings);
            Guard.ArgumentNotNull("clientScripts", clientScriptsSettings);
            Guard.ArgumentNotNull("inspectorsSettings", inspectorsSettings);
            Guard.ArgumentNotNull("resourcesSettings", resourcesSettings);
            Guard.ArgumentNotNull("resourcesSettings", resourcesSettings);
            Guard.ArgumentNotNull("runtimePoliciesSettings", runtimePoliciesSettings);
            Guard.ArgumentNotNull("tabsSettings", tabsSettings);
            Guard.ArgumentNotNull("serializationConvertersSettings", serializationConvertersSettings);
            Guard.ArgumentNotNull("metadataSettings", metadataSettings);
            Guard.ArgumentNotNull("tabMetadataSettings", tabMetadataSettings);
            Guard.ArgumentNotNull("displaysSettings", displaysSettings);
            Guard.ArgumentNotNull("instanceMetadataSettings", instanceMetadataSettings);

            ResourceEndpointConfiguration = resourceEndpointConfiguration;
            PersistenceStore = persistenceStore;
            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker;
            DefaultRuntimePolicy = defaultRuntimePolicy;
            EndpointBaseUri = endpointBaseUri;
            LoggingSettings = loggingSettings;
            ClientScriptsSettings = clientScriptsSettings;
            InspectorsSettings = inspectorsSettings;
            ResourcesSettings = resourcesSettings;
            RuntimePoliciesSettings = runtimePoliciesSettings;
            TabsSettings = tabsSettings;
            SerializationConvertersSettings = serializationConvertersSettings;
            MetadataSettings = metadataSettings;
            TabMetadataSettings = tabMetadataSettings;
            DisplaysSettings = displaysSettings;
            InstanceMetadataSettings = instanceMetadataSettings;
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
        /// Gets the logging settings
        /// </summary>
        public LoggingSettings LoggingSettings { get; private set; }

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
        /// Gets the client scripts settings
        /// </summary>
        public CollectionSettings ClientScriptsSettings { get; private set; }

        /// <summary>
        /// Gets the inspectors settings
        /// </summary>
        public CollectionSettings InspectorsSettings { get; private set; }

        /// <summary>
        /// Gets the resources settings
        /// </summary>
        public CollectionSettings ResourcesSettings { get; private set; }

        /// <summary>
        /// Gets the runtime policies settings
        /// </summary>
        public CollectionSettings RuntimePoliciesSettings { get; private set; }

        /// <summary>
        /// Gets the tabs settings
        /// </summary>
        public CollectionSettings TabsSettings { get; private set; }

        /// <summary>
        /// Gets the serialization converters settings
        /// </summary>
        public CollectionSettings SerializationConvertersSettings { get; private set; }

        /// <summary>
        /// Gets the metadata settings
        /// </summary>
        public CollectionSettings MetadataSettings { get; private set; }

        /// <summary>
        /// Gets the tab metadata settings
        /// </summary>
        public CollectionSettings TabMetadataSettings { get; private set; }

        /// <summary>
        /// Gets the display settings
        /// </summary>
        public CollectionSettings DisplaysSettings { get; private set; }

        /// <summary>
        /// Gets the instance metadata settings
        /// </summary>
        public CollectionSettings InstanceMetadataSettings { get; private set; }
    }
}