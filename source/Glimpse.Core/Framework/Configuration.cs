using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Contains all configuration required by <see cref="IGlimpseRuntime"/> instances to execute.
    /// </summary>
    public class Configuration : IConfiguration
    {
        private IMessageBroker messageBroker;
        private ILogger logger;
        private ICollection<IClientScript> clientScripts;
        private IResource defaultResource;
        private string endpointBaseUri;
        private IHtmlEncoder htmlEncoder;
        private IPersistenceStore persistenceStore;
        private ICollection<IInspector> inspectors;
        private IProxyFactory proxyFactory;
        private IResourceEndpointConfiguration resourceEndpoint;
        private ICollection<IResource> resources;
        private ICollection<IRuntimePolicy> runtimePolicies;
        private ISerializer serializer;
        private ICollection<ITab> tabs;
        private ICollection<IMetadata> metadata;
        private ICollection<ITabMetadata> tabMetadata;
        private ICollection<IInstanceMetadata> instanceMetadata;
        private ICollection<IDisplay> displays;
        private string hash;
        private string version;
        private Section xmlConfiguration;
        private RuntimePolicy? defaultRuntimePolicy;
        private ICollection<ISerializationConverter> serializationConverters;

        public Configuration(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
            : this(endpointConfiguration, persistenceStore, "glimpse", currentGlimpseRequestIdTracker)
        {
        }

        public Configuration(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, string xmlConfigurationSectionName, ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
            : this(endpointConfiguration, persistenceStore, ConfigurationManager.GetSection(xmlConfigurationSectionName) as Section, currentGlimpseRequestIdTracker)
        {
        }

        public Configuration(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, Section xmlConfigurationSection, ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            if (endpointConfiguration == null)
            {
                throw new ArgumentNullException("endpointConfiguration");
            }

            if (persistenceStore == null)
            {
                throw new ArgumentNullException("persistenceStore");
            }

            if (xmlConfigurationSection == null)
            {
                throw new ArgumentNullException("xmlConfigurationSection");
            }

            ResourceEndpoint = endpointConfiguration;
            PersistenceStore = persistenceStore;
            XmlConfiguration = xmlConfigurationSection;
            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker();
        }

        /// <summary>
        /// Gets the <see cref="ICurrentGlimpseRequestIdTracker"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ICurrentGlimpseRequestIdTracker"/>.
        /// </value>
        public ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; private set; }

        public Section XmlConfiguration 
        {
            get
            {
                return xmlConfiguration;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                xmlConfiguration = value;
            }
        }

        /// <summary>
        /// Gets or sets the client scripts collection.
        /// </summary>
        /// <value>
        /// The client scripts.
        /// </value>
        /// <returns>A collection of <see cref="IClientScript"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IClientScript> ClientScripts
        {
            get
            {
                if (clientScripts != null)
                {
                    return clientScripts;
                }

                clientScripts = InstantiateDiscoverableCollection<IClientScript>(XmlConfiguration.ClientScripts);
                return clientScripts;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                clientScripts = value;
            }
        }

        /// <summary>
        /// Gets or sets the default <see cref="IResource"/> to execute.
        /// </summary>
        /// <value>
        /// The default resource.
        /// </value>
        /// <returns>A <see cref="IResource"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IResource DefaultResource
        {
            get
            {
                if (defaultResource != null)
                {
                    return defaultResource;
                }

                defaultResource = new ConfigurationResource();
                return defaultResource;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                defaultResource = value;
            }
        }

        /// <summary>
        /// Gets or sets the default runtime policy.
        /// </summary>
        /// <value>
        /// The default runtime policy.
        /// </value>
        /// <returns>A <see cref="RuntimePolicy"/> instance based on configuration settings.</returns>
        public RuntimePolicy DefaultRuntimePolicy 
        {
            get
            {
                if (defaultRuntimePolicy.HasValue)
                {
                    return defaultRuntimePolicy.Value;
                }

                defaultRuntimePolicy = XmlConfiguration.DefaultRuntimePolicy;
                return defaultRuntimePolicy.Value;
            }

            set
            {
                defaultRuntimePolicy = value;
            }
        }

        /// <summary>
        /// Gets or sets the endpoint base URI.
        /// </summary>
        /// <value>
        /// The endpoint base URI.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public string EndpointBaseUri
        {
            get
            {
                if (!string.IsNullOrEmpty(endpointBaseUri))
                {
                    return endpointBaseUri;
                }

                endpointBaseUri = XmlConfiguration.EndpointBaseUri;
                return endpointBaseUri;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("EndpointBaseUri must be a non-null, non-empty string.", "value");
                }

                endpointBaseUri = value;
            }
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IHtmlEncoder"/>.
        /// </summary>
        /// <returns>A <see cref="IHtmlEncoder"/> instance (leveraging the <see href="http://wpl.codeplex.com/">Microsoft Web Protection Library</see>).</returns>
        public IHtmlEncoder HtmlEncoder
        {
            get
            {
                if (htmlEncoder != null)
                {
                    return htmlEncoder;
                }

                htmlEncoder = new AntiXssEncoder();
                return htmlEncoder;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                htmlEncoder = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ILogger"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ILogger"/>.
        /// </value>
        /// <returns>A <see cref="ILogger"/> instance (leveraging the <see href="http://nlog-project.org/">NLog</see> project) based on configuration settings.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ILogger Logger
        {
            get
            {
                if (logger != null)
                {
                    return logger;
                }

                // use null logger if logging is off
                var logLevel = XmlConfiguration.Logging.Level;
                if (logLevel == LoggingLevel.Off)
                {
                    logger = new NullLogger();
                    return logger;
                }

                logger = CreateLogger();
                return logger;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                logger = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IMessageBroker"/>.
        /// </summary>
        /// <returns>A <see cref="IMessageBroker"/> instance.</returns>
        /// <value>
        /// The configured <see cref="IMessageBroker"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IMessageBroker MessageBroker
        {
            get
            {
                if (messageBroker != null)
                {
                    return messageBroker;
                }

                messageBroker = new MessageBroker(
                    () => GlimpseRuntime.IsAvailable && GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy != RuntimePolicy.Off,
                    Logger);

                return messageBroker;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                messageBroker = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IPersistenceStore"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IPersistenceStore"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IPersistenceStore PersistenceStore
        {
            get
            {
                return persistenceStore;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                persistenceStore = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IInspector"/>.
        /// </summary>
        /// <value>
        /// The configured collection of <see cref="IInspector"/>.
        /// </value>
        /// <returns>A collection of <see cref="IInspector"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IInspector> Inspectors
        {
            get
            {
                if (inspectors != null)
                {
                    return inspectors;
                }

                inspectors = InstantiateDiscoverableCollection<IInspector>(XmlConfiguration.Inspectors);
                return inspectors;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                inspectors = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IProxyFactory"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IProxyFactory"/>.
        /// </value>
        /// <returns>A <see cref="IProxyFactory"/> instance (leveraging <see href="http://www.castleproject.org/projects/dynamicproxy/">Castle DynamicProxy</see>.).</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IProxyFactory ProxyFactory
        {
            get
            {
                if (proxyFactory != null)
                {
                    return proxyFactory;
                }

                proxyFactory = new CastleDynamicProxyFactory(
                    Logger,
                    MessageBroker,
                    () => GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer : UnavailableGlimpseRequestContext.Instance.CurrentExecutionTimer,
                    () => GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy : UnavailableGlimpseRequestContext.Instance.CurrentRuntimePolicy);

                return proxyFactory;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                proxyFactory = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IResourceEndpointConfiguration"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IResourceEndpointConfiguration"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IResourceEndpointConfiguration ResourceEndpoint
        {
            get
            {
                return resourceEndpoint;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                resourceEndpoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IResource"/>.
        /// </summary>
        /// <value>
        /// The configured collection of <see cref="IResource"/>.
        /// </value>
        /// <returns>A collection of <see cref="IResource"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IResource> Resources
        {
            get
            {
                if (resources != null)
                {
                    return resources;
                }

                resources = InstantiateDiscoverableCollection<IResource>(XmlConfiguration.Resources);
                return resources;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                resources = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IRuntimePolicy"/>.
        /// </summary>
        /// <value>
        /// The configured collection of <see cref="IRuntimePolicy"/>.
        /// </value>
        /// <returns>A collection of <see cref="IRuntimePolicy"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IRuntimePolicy> RuntimePolicies
        {
            get
            {
                if (runtimePolicies != null)
                {
                    return runtimePolicies;
                }

                runtimePolicies = InstantiateDiscoverableCollection<IRuntimePolicy>(XmlConfiguration.RuntimePolicies);
                return runtimePolicies;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                runtimePolicies = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISerializer"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ISerializer"/>.
        /// </value>
        /// <returns>A <see cref="ISerializer"/> instance (leveraging <see href="http://json.codeplex.com/">Json.Net</see>).</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ISerializer Serializer
        {
            get
            {
                if (serializer != null)
                {
                    return serializer;
                }

                var temp = new JsonNetSerializer(Logger);
                temp.RegisterSerializationConverters(SerializationConverters);

                serializer = temp;
                return serializer;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                serializer = value;
            }
        }
        
        /// <summary>
        /// Gets or sets a collection of <see cref="ISerializationConverter"/>s.
        /// </summary>
        /// <returns>A collection of <see cref="ISerializationConverter"/> instances discovered in the configured discovery location.</returns>
        public ICollection<ISerializationConverter> SerializationConverters 
        {
            get
            {
                if (serializationConverters != null)
                {
                    return serializationConverters;
                }

                serializationConverters = InstantiateDiscoverableCollection<ISerializationConverter>(XmlConfiguration.SerializationConverters);
                return serializationConverters;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                serializationConverters = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ITab"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ITab"/>.
        /// </value>
        /// <returns>A collection of <see cref="ITab"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<ITab> Tabs
        {
            get
            {
                if (tabs != null)
                {
                    return tabs;
                }

                tabs = InstantiateDiscoverableCollection<ITab>(XmlConfiguration.Tabs);
                return tabs;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                tabs = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IMetadata"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IMetadata"/>.
        /// </value>
        /// <returns>A collection of <see cref="IMetadata"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IMetadata> Metadata
        {
            get
            {
                if (metadata != null)
                {
                    return metadata;
                }

                metadata = CreateDiscoverableCollection<IMetadata>(XmlConfiguration.Metadata);
                return metadata;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                metadata = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ITabMetadata"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ITabMetadata"/>.
        /// </value>
        /// <returns>A collection of <see cref="ITabMetadata"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<ITabMetadata> TabMetadata
        {
            get
            {
                if (tabMetadata != null)
                {
                    return tabMetadata;
                }

                tabMetadata = CreateDiscoverableCollection<ITabMetadata>(XmlConfiguration.TabMetadata);
                return tabMetadata;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                tabMetadata = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IInstanceMetadata"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IInstanceMetadata"/>.
        /// </value>
        /// <returns>A collection of <see cref="IInstanceMetadata"/> instances discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IInstanceMetadata> InstanceMetadata
        {
            get
            {
                if (instanceMetadata != null)
                {
                    return instanceMetadata;
                }

                instanceMetadata = CreateDiscoverableCollection<IInstanceMetadata>(XmlConfiguration.InstanceMetadata);
                return instanceMetadata;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                instanceMetadata = value;
            }
        }

        public ICollection<IDisplay> Displays
        {
            get
            {
                if (displays != null)
                {
                    return displays;
                }

                displays = InstantiateDiscoverableCollection<IDisplay>(XmlConfiguration.Displays);
                return displays;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                displays = value;
            }
        }

        public string Hash
        {
            get
            {
                if (!string.IsNullOrEmpty(hash))
                {
                    return hash;
                }

                var configuredTypes = new List<Type> { GetType() };
                configuredTypes.AddRange(Tabs.Select(tab => tab.GetType()).OrderBy(type => type.Name));
                configuredTypes.AddRange(Inspectors.Select(inspector => inspector.GetType()).OrderBy(type => type.Name));
                configuredTypes.AddRange(Resources.Select(resource => resource.GetType()).OrderBy(type => type.Name));
                configuredTypes.AddRange(ClientScripts.Select(clientScript => clientScript.GetType()).OrderBy(type => type.Name));
                configuredTypes.AddRange(RuntimePolicies.Select(policy => policy.GetType()).OrderBy(type => type.Name));
                configuredTypes.AddRange(Metadata.Select(extensions => extensions.GetType()).OrderBy(type => type.Name));
                configuredTypes.AddRange(TabMetadata.Select(extensions => extensions.GetType()).OrderBy(type => type.Name));

                var crc32 = new Crc32();
                var sb = new StringBuilder();
                using (var memoryStream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, configuredTypes);
                    memoryStream.Position = 0;

                    var computeHash = crc32.ComputeHash(memoryStream);

                    foreach (var b in computeHash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                }

                hash = sb.ToString();
                return hash;
            }

            set
            {
                hash = value;
            }
        }

        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(version))
                {
                    version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
                }
                 
                return version;
            }

            set
            {
                version = value;
            }
        } 

        public void ApplyOverrides()
        {
            // This method can be updated to ensure that web.config settings "win" - but that is difficult to do for most of them
            DefaultRuntimePolicy = XmlConfiguration.DefaultRuntimePolicy;
            EndpointBaseUri = XmlConfiguration.EndpointBaseUri;
            if (XmlConfiguration.Logging.Level != LoggingLevel.Off)
            {
                Logger = CreateLogger();
            }
        }

        private ILogger CreateLogger()
        {
            // Root the path if it isn't already and add a filename if one isn't specified
            var configuredPath = XmlConfiguration.Logging.LogLocation;
            var logDirPath = Path.IsPathRooted(configuredPath) ? configuredPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuredPath);
            var logFilePath = string.IsNullOrEmpty(Path.GetExtension(logDirPath)) ? Path.Combine(logDirPath, "Glimpse.log") : logDirPath;
 
            var fileTarget = new FileTarget();
            fileTarget.FileName = logFilePath;
            fileTarget.Layout = "${longdate} | ${level:uppercase=true} | ${message} | ${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method:innerExceptionSeparator=>>}";

            var asyncTarget = new AsyncTargetWrapper(fileTarget); 
            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget("file", asyncTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int)XmlConfiguration.Logging.Level), asyncTarget));

            return new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
        }

        private ICollection<T> CreateDiscoverableCollection<T>(DiscoverableCollectionElement config)
        {
            return DiscoverableCollectionFactory.Create<T>(config, XmlConfiguration.DiscoveryLocation, Logger);
        }
        
        private ICollection<TElementType> InstantiateDiscoverableCollection<TElementType>(DiscoverableCollectionElement configuredDiscoverableCollection)
            where TElementType : class
        {
            var discoverableCollection = CreateDiscoverableCollection<TElementType>(configuredDiscoverableCollection);

            // get the list of configurators
            var configurators = discoverableCollection.OfType<IConfigurable>()
                .Select(configurable => configurable.Configurator)
                .GroupBy(configurator => configurator.CustomConfigurationKey);

            // have each configurator, configure its "configurable"
            foreach (var groupedConfigurators in configurators)
            {
                if (groupedConfigurators.Count() != 1)
                {
                    // there are multiple configurators using the same custom configuration key inside the same discoverable collection
                    // this means that any existing custom configuration content must be resolved by using the custom configuration key
                    // and the type for which the configurator is
                    foreach (var configurator in groupedConfigurators)
                    {
                        string customConfiguration = configuredDiscoverableCollection.GetCustomConfiguration(configurator.CustomConfigurationKey, configurator.GetType());
                        if (!string.IsNullOrEmpty(customConfiguration))
                        {
                            configurator.ProcessCustomConfiguration(customConfiguration);
                        }
                    }
                }
                else
                {
                    var configurator = groupedConfigurators.Single();
                    string customConfiguration = configuredDiscoverableCollection.GetCustomConfiguration(configurator.CustomConfigurationKey);
                    if (!string.IsNullOrEmpty(customConfiguration))
                    {
                        configurator.ProcessCustomConfiguration(customConfiguration);
                    }
                }
            }

            return discoverableCollection;
        }
    }
}
