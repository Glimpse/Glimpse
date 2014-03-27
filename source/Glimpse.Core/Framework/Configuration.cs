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
    internal class Configuration : IConfiguration
    {
        private string hash;

        public Configuration(IResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
            : this(endpointConfiguration, persistenceStore, "glimpse", currentGlimpseRequestIdTracker)
        {
        }

        public Configuration(IResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, string xmlConfigurationSectionName, ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
            : this(endpointConfiguration, persistenceStore, ConfigurationManager.GetSection(xmlConfigurationSectionName) as Section, currentGlimpseRequestIdTracker)
        {
        }

        public Configuration(IResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, Section xmlConfiguration, ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker = null)
        {
            Guard.ArgumentNotNull("endpointConfiguration", endpointConfiguration);
            Guard.ArgumentNotNull("persistenceStore", persistenceStore);
            Guard.ArgumentNotNull("xmlConfiguration", xmlConfiguration);

            ResourceEndpoint = endpointConfiguration;
            PersistenceStore = persistenceStore;
            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker ?? new CallContextCurrentGlimpseRequestIdTracker();

#warning we kinda have a chicken and egg problem concerning the configuration, although if we say the config can be used to troubleshoot issues, then it might make sense
#warning why the problem? well you need to fill all collections, as they may want to do some look ups or alter collections, yet that means that things have already been loaded
#warning although that is something users can fix by setting discoverable to false in the config, no issue then...

            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            DefaultResource = new ConfigurationResource();
            DefaultRuntimePolicy = xmlConfiguration.DefaultRuntimePolicy;
            EndpointBaseUri = xmlConfiguration.EndpointBaseUri;
            HtmlEncoder = new AntiXssEncoder();
            Logger = CreateLogger(xmlConfiguration.Logging);
#warning the logger we need here must be a wrapper around the real logger, because it is being passed in into the other collections and types, and if the logger should be replaced, then it wouldn't be used by those since they still have the old one referenced

            MessageBroker = new MessageBroker(
                    () => GlimpseRuntime.IsAvailable && GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy != RuntimePolicy.Off,
                    Logger);

            ProxyFactory = new CastleDynamicProxyFactory(
                    Logger,
                    MessageBroker,
                    () => GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer : UnavailableGlimpseRequestContext.Instance.CurrentExecutionTimer,
                    () => GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy : UnavailableGlimpseRequestContext.Instance.CurrentRuntimePolicy);


            ClientScripts = new ClientScriptsCollection(
                new CollectionConfigurationFactory(xmlConfiguration.ClientScripts.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            Inspectors = new InspectorsCollection(
                new CollectionConfigurationFactory(xmlConfiguration.Inspectors.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            Resources = new ResourcesCollection(
                new CollectionConfigurationFactory(xmlConfiguration.Resources.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            RuntimePolicies = new RuntimePoliciesCollection(
                new CollectionConfigurationFactory(xmlConfiguration.RuntimePolicies.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            Tabs = new TabsCollection(
                new CollectionConfigurationFactory(xmlConfiguration.Tabs.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            SerializationConverters = new SerializationConvertersCollection(
                new CollectionConfigurationFactory(xmlConfiguration.SerializationConverters.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            var temp = new JsonNetSerializer(Logger);
            temp.RegisterSerializationConverters(SerializationConverters);
            Serializer = temp;

            Metadata = new MetadataCollection(
                new CollectionConfigurationFactory(xmlConfiguration.Metadata.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            TabMetadata = new TabMetadataCollection(
                new CollectionConfigurationFactory(xmlConfiguration.TabMetadata.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            Displays = new DisplaysCollection(
                new CollectionConfigurationFactory(xmlConfiguration.Displays.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            InstanceMetadata = new InstanceMetadataCollection(
                new CollectionConfigurationFactory(xmlConfiguration.InstanceMetadata.XmlContent, xmlConfiguration.DiscoveryLocation).Create(),
                Logger);

            // TODO: Instantiate the user's IOC container (if they have one)
        }

        /// <summary>
        /// Gets the <see cref="ICurrentGlimpseRequestIdTracker"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ICurrentGlimpseRequestIdTracker"/>.
        /// </value>
        public ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; private set; }

        public IConfiguration ReplaceCurrentGlimpseRequestIdTracker(ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker)
        {
            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker;
            return this;
        }

        /// <summary>
        /// Gets or sets the client scripts collection.
        /// </summary>
        /// <value>
        /// The client scripts.
        /// </value>
        /// <returns>A collection of <see cref="IClientScript"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IClientScript"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IClientScript> ClientScripts { get; private set; }

        /// <summary>
        /// Gets or sets the default <see cref="IResource"/> to execute.
        /// </summary>
        /// <value>
        /// The default resource.
        /// </value>
        /// <returns>A <see cref="IResource"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="ConfigurationResource"/>.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IResource DefaultResource { get; private set; }

        public IConfiguration ReplaceDefaultResource(IResource defaultResource)
        {
            DefaultResource = defaultResource;
            return this;
        }

        /// <summary>
        /// Gets or sets the default runtime policy.
        /// </summary>
        /// <value>
        /// The default runtime policy.
        /// </value>
        /// <returns>A <see cref="RuntimePolicy"/> instance based on configuration settings.</returns>
        public RuntimePolicy DefaultRuntimePolicy { get; private set; }

        /// <summary>
        /// Gets or sets the endpoint base URI.
        /// </summary>
        /// <value>
        /// The endpoint base URI.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public string EndpointBaseUri { get; private set; }

        /// <summary>
        /// Instantiates an instance of <see cref="IHtmlEncoder"/>.
        /// </summary>
        /// <returns>A <see cref="IHtmlEncoder"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="AntiXssEncoder"/> (leveraging the <see href="http://wpl.codeplex.com/">Microsoft Web Protection Library</see>).</returns>
        public IHtmlEncoder HtmlEncoder { get; private set; }

        public IConfiguration ReplaceHtmlEncoder(IHtmlEncoder htmlEncoder)
        {
            HtmlEncoder = htmlEncoder;
            return this;
        }

        /// <summary>
        /// Gets or sets the <see cref="ILogger"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ILogger"/>.
        /// </value>
        /// <returns>A <see cref="ILogger"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise a <see cref="NullLogger"/> or <see cref="NLogLogger"/> (leveraging the <see href="http://nlog-project.org/">NLog</see> project) based on configuration settings.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ILogger Logger { get; private set; }

        public IConfiguration ReplaceLogger(ILogger logger)
        {
#warning make sure to dispose the old one, before replacing it with the new one... beware, we should not store the Logger here but assign it to a LoggerWrapper -> see constructor for more details
            Logger = logger;
            return this;
        }

        /// <summary>
        /// Gets or sets the <see cref="IMessageBroker"/>.
        /// </summary>
        /// <returns>A <see cref="IMessageBroker"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="MessageBroker"/>.</returns>
        /// <value>
        /// The configured <see cref="IMessageBroker"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IMessageBroker MessageBroker { get; private set; }

        public IConfiguration ReplaceMessageBroker(IMessageBroker messageBroker)
        {
            MessageBroker = messageBroker;
            return this;
        }

        /// <summary>
        /// Gets or sets the <see cref="IPersistenceStore"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IPersistenceStore"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IPersistenceStore PersistenceStore { get; private set; }

        public IConfiguration ReplacePersistenceStore(IPersistenceStore persistenceStore)
        {
            PersistenceStore = persistenceStore;
            return this;
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IInspector"/>.
        /// </summary>
        /// <value>
        /// The configured collection of <see cref="IInspector"/>.
        /// </value>
        /// <returns>A collection of <see cref="IInspector"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IInspector"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IInspector> Inspectors { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="IProxyFactory"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IProxyFactory"/>.
        /// </value>
        /// <returns>A <see cref="IProxyFactory"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="CastleDynamicProxyFactory"/> (leveraging <see href="http://www.castleproject.org/projects/dynamicproxy/">Castle DynamicProxy</see>.).</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IProxyFactory ProxyFactory { get; private set; }

        public IConfiguration ReplaceProxyFactory(IProxyFactory proxyFactory)
        {
            ProxyFactory = proxyFactory;
            return this;
        }

        /// <summary>
        /// Gets or sets the <see cref="IResourceEndpointConfiguration"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IResourceEndpointConfiguration"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IResourceEndpointConfiguration ResourceEndpoint { get; private set; }

        public IConfiguration ReplaceResourceEndpoint(IResourceEndpointConfiguration resourceEndpointConfiguration)
        {
            ResourceEndpoint = resourceEndpointConfiguration;
            return this;
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IResource"/>.
        /// </summary>
        /// <value>
        /// The configured collection of <see cref="IResource"/>.
        /// </value>
        /// <returns>A collection of <see cref="IResource"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IResource"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IResource> Resources { get; private set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IRuntimePolicy"/>.
        /// </summary>
        /// <value>
        /// The configured collection of <see cref="IRuntimePolicy"/>.
        /// </value>
        /// <returns>A collection of <see cref="IRuntimePolicy"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IRuntimePolicy"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IRuntimePolicy> RuntimePolicies { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ISerializer"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ISerializer"/>.
        /// </value>
        /// <returns>A <see cref="ISerializer"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="JsonNetSerializer"/> (leveraging <see href="http://json.codeplex.com/">Json.Net</see>).</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ISerializer Serializer { get; private set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="ISerializationConverter"/>s.
        /// </summary>
        /// <returns>A collection of <see cref="ISerializationConverter"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ISerializationConverter"/>s discovered in the configured discovery location.</returns>
        public ICollection<ISerializationConverter> SerializationConverters { get; private set; }

        public IConfiguration ReplaceSerializer(ISerializer serializer)
        {
            Serializer = serializer;
            return this;
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ITab"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ITab"/>.
        /// </value>
        /// <returns>A collection of <see cref="ITab"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ITab"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<ITab> Tabs { get; private set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IMetadata"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IMetadata"/>.
        /// </value>
        /// <returns>A collection of <see cref="IMetadata"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ITab"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IMetadata> Metadata { get; private set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ITabMetadata"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ITabMetadata"/>.
        /// </value>
        /// <returns>A collection of <see cref="ITabMetadata"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ITab"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<ITabMetadata> TabMetadata { get; private set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IInstanceMetadata"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="IInstanceMetadata"/>.
        /// </value>
        /// <returns>A collection of <see cref="IInstanceMetadata"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ITab"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IInstanceMetadata> InstanceMetadata { get; private set; }

        public ICollection<IDisplay> Displays { get; private set; }

        public string Hash { get { return hash = hash ?? GenerateHash(); } }

        private string GenerateHash()
        {
#warning the way the hash is being calculated, doesn't apply anymore if configuration can be changed afterwards, the hash should be stable

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

            return sb.ToString();
        }

        public string Version { get; private set; }

        private ILogger CreateLogger(LoggingElement loggingElement)
        {
            // use null logger if logging is off
            if (loggingElement.Level == LoggingLevel.Off)
            {
                return new NullLogger();
            }

            // Root the path if it isn't already and add a filename if one isn't specified
            var configuredPath = loggingElement.LogLocation;
            var logDirPath = Path.IsPathRooted(configuredPath) ? configuredPath : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuredPath);
            var logFilePath = string.IsNullOrEmpty(Path.GetExtension(logDirPath)) ? Path.Combine(logDirPath, "Glimpse.log") : logDirPath;

            var fileTarget = new FileTarget();
            fileTarget.FileName = logFilePath;
            fileTarget.Layout = "${longdate} | ${level:uppercase=true} | ${message} | ${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method:innerExceptionSeparator=>>}";

            var asyncTarget = new AsyncTargetWrapper(fileTarget);
            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget("file", asyncTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int)loggingElement.Level), asyncTarget));

            return new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
        }
    }
}