using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Contains all configuration required by <see cref="IGlimpseRuntime"/> instances to execute.
    /// </summary>
    internal class Configuration : IConfiguration
    {
        private readonly LoggerWrapper LoggerWrapper;

        public Configuration(ConfigurationSettings configurationSettings)
        {
            Guard.ArgumentNotNull("configurationSettings", configurationSettings);

            ResourceEndpoint = configurationSettings.ResourceEndpointConfiguration;
            PersistenceStore = configurationSettings.PersistenceStore;
            CurrentGlimpseRequestIdTracker = configurationSettings.CurrentGlimpseRequestIdTracker;
            DefaultRuntimePolicy = configurationSettings.DefaultRuntimePolicy;
            EndpointBaseUri = configurationSettings.EndpointBaseUri;

            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            LoggerWrapper = new LoggerWrapper(configurationSettings.Logging.Level, configurationSettings.Logging.LogLocation);

            DefaultResource = new ConfigurationResource();
            HtmlEncoder = new AntiXssEncoder();

            MessageBroker = new MessageBroker(
                    () => GlimpseRuntime.IsAvailable && GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy != RuntimePolicy.Off,
                    Logger);

            ProxyFactory = new CastleDynamicProxyFactory(
                    Logger,
                    MessageBroker,
                    () => GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer : UnavailableGlimpseRequestContext.Instance.CurrentExecutionTimer,
                    () => GlimpseRuntime.IsAvailable ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy : UnavailableGlimpseRequestContext.Instance.CurrentRuntimePolicy);

            ClientScripts = new ClientScriptsCollection(configurationSettings.ClientScriptsConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            Inspectors = new InspectorsCollection(configurationSettings.InspectorsConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            Resources = new ResourcesCollection(configurationSettings.ResourcesConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            RuntimePolicies = new RuntimePoliciesCollection(configurationSettings.RuntimePoliciesConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            Tabs = new TabsCollection(configurationSettings.TabsConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            SerializationConverters = new SerializationConvertersCollection(configurationSettings.SerializationConvertersConfiguration, Logger);
            Metadata = new MetadataCollection(configurationSettings.MetadataConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            TabMetadata = new TabMetadataCollection(configurationSettings.TabMetadataConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            Displays = new DisplaysCollection(configurationSettings.DisplaysConfiguration, Logger, (sender, e) => GenerateAndStoreHash());
            InstanceMetadata = new InstanceMetadataCollection(configurationSettings.InstanceMetadataConfiguration, Logger, (sender, e) => GenerateAndStoreHash());

            var temp = new JsonNetSerializer(Logger);
            temp.RegisterSerializationConverters(SerializationConverters);
            Serializer = temp;

            GenerateAndStoreHash();

            // TODO: Instantiate the user's IOC container (if they have one)
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
        /// Gets the current version of the Glimpse core assembly.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; private set; }

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
        /// Gets configured <see cref="ILogger"/>.
        /// </summary>
        /// <returns>The configured <see cref="ILogger"/> which defaults to a <see cref="NullLogger" /> in case the configured log level is set 
        /// to <see cref="LoggingLevel.Off"/> or with a <see cref="NLog.Logger" /> (leveraging the <see href="http://nlog-project.org/">NLog</see> project)
        /// for all other log levels. The configured logger can be replaced with a call to <see cref="ReplaceLogger"/>.
        /// </returns>
        public ILogger Logger { get { return LoggerWrapper; } }

        /// <summary>
        /// Replaces the current configured logger with the <paramref name="logger" /> specified.
        /// Keep in mind that the <paramref name="logger" /> will be ignored if the configured log level is set to <see cref="LoggingLevel.Off"/>
        /// </summary>
        /// <param name="logger">The logger to use from now on</param>
        /// <returns>This configuration</returns>
        public IConfiguration ReplaceLogger(ILogger logger)
        {
            LoggerWrapper.SwitchLogger(logger);
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

        public string Hash { get; private set; }

        private void GenerateAndStoreHash()
        {
            var configuredTypes = new List<Type> { GetType() };
            configuredTypes.AddRange(Tabs.Select(tab => tab.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(Inspectors.Select(inspector => inspector.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(Resources.Select(resource => resource.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(ClientScripts.Select(clientScript => clientScript.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(RuntimePolicies.Select(policy => policy.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(Metadata.Select(extensions => extensions.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(TabMetadata.Select(extensions => extensions.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(InstanceMetadata.Select(extensions => extensions.GetType()).OrderBy(type => type.Name));
            configuredTypes.AddRange(Displays.Select(extensions => extensions.GetType()).OrderBy(type => type.Name));

            Hash = HashGenerator.Generate(configuredTypes);
        }
    }
}