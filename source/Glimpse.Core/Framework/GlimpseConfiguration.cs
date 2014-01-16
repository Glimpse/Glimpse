using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public class GlimpseConfiguration : IGlimpseConfiguration
    {
        private IMessageBroker messageBroker;
        private Func<IExecutionTimer> timerStrategy;
        private ILogger logger;
        private ICollection<IClientScript> clientScripts;
        private IResource defaultResource;
        private string endpointBaseUri;
        private IHtmlEncoder htmlEncoder;
        private IPersistenceStore persistenceStore;
        private ICollection<IInspector> inspectors;
        private IProxyFactory proxyFactory;
        private ResourceEndpointConfiguration resourceEndpoint;
        private ICollection<IResource> resources;
        private ICollection<IRuntimePolicy> runtimePolicies;
        private ISerializer serializer;
        private ICollection<ITab> tabs;
        private ICollection<IDisplay> displays;
        private Func<RuntimePolicy> runtimePolicyStrategy;
        private string hash;
        private IServiceLocator userServiceLocator;
        private Section xmlConfiguration;
        private RuntimePolicy? defaultRuntimePolicy;
        private ICollection<ISerializationConverter> serializationConverters;

        public GlimpseConfiguration(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            if (endpointConfiguration == null)
            {
                throw new ArgumentNullException("endpointConfiguration");
            }

            if (persistenceStore == null)
            {
                throw new ArgumentNullException("persistenceStore");
            }

            ResourceEndpoint = endpointConfiguration;
            PersistenceStore = persistenceStore;
            // TODO: Instantiate the user's IOC container (if they have one)
        }

        public IServiceLocator UserServiceLocator 
        {
            get { return userServiceLocator; }
            set { userServiceLocator = value; }
        }

        public Section XmlConfiguration {
            get
            {
                if (xmlConfiguration != null)
                {
                    return xmlConfiguration;
                }

                xmlConfiguration = ConfigurationManager.GetSection("glimpse") as Section ?? new Section();
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
        /// <returns>A collection of <see cref="IClientScript"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IClientScript"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IClientScript> ClientScripts
        {
            get
            {
                if (clientScripts != null)
                {
                    return clientScripts;
                }

                if (TryAllInstancesFromServiceLocators(out clientScripts))
                {
                    return clientScripts;
                }

                clientScripts = CreateDiscoverableCollection<IClientScript>(XmlConfiguration.ClientScripts);
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
        /// <returns>A <see cref="IResource"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="ConfigurationResource"/>.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IResource DefaultResource
        {
            get
            {
                if (defaultResource != null)
                {
                    return defaultResource;
                }

                if (TrySingleInstanceFromServiceLocators(out defaultResource))
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
        /// <returns>A <see cref="IHtmlEncoder"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="AntiXssEncoder"/> (leveraging the <see href="http://wpl.codeplex.com/">Microsoft Web Protection Library</see>).</returns>
        public IHtmlEncoder HtmlEncoder
        {
            get
            {
                if (htmlEncoder != null)
                {
                    return htmlEncoder;
                }

                if (TrySingleInstanceFromServiceLocators(out htmlEncoder))
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
        /// <returns>A <see cref="ILogger"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise a <see cref="NullLogger"/> or <see cref="NLogLogger"/> (leveraging the <see href="http://nlog-project.org/">NLog</see> project) based on configuration settings.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ILogger Logger
        {
            get
            {
                if (logger != null)
                {
                    return logger;
                }

                if (TrySingleInstanceFromServiceLocators(out logger))
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

                var configuredPath = XmlConfiguration.Logging.LogLocation;

                // Root the path if it isn't already
                var logDirPath = Path.IsPathRooted(configuredPath)
                                     ? configuredPath
                                     : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuredPath);

                // Add a filename if one isn't specified
                var logFilePath = string.IsNullOrEmpty(Path.GetExtension(logDirPath))
                                      ? Path.Combine(logDirPath, "Glimpse.log")
                                      : logDirPath;

                // use NLog logger otherwise
                var fileTarget = new FileTarget
                {
                    FileName = logFilePath,
                    Layout =
                        "${longdate} | ${level:uppercase=true} | ${message} | ${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method:innerExceptionSeparator=>>}"
                };

                var asyncTarget = new AsyncTargetWrapper(fileTarget);

                var loggingConfiguration = new LoggingConfiguration();
                loggingConfiguration.AddTarget("file", asyncTarget);
                loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int)logLevel), asyncTarget));

                logger = new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
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
        /// <returns>A <see cref="IMessageBroker"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="MessageBroker"/>.</returns>
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

                if (TrySingleInstanceFromServiceLocators(out messageBroker))
                {
                    return messageBroker;
                }

                messageBroker = new MessageBroker(Logger);
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
        /// <returns>A collection of <see cref="IInspector"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IInspector"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IInspector> Inspectors
        {
            get
            {
                if (inspectors != null)
                {
                    return inspectors;
                }

                if (TryAllInstancesFromServiceLocators(out inspectors))
                {
                    return inspectors;
                }

                inspectors = CreateDiscoverableCollection<IInspector>(XmlConfiguration.Inspectors);
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
        /// <returns>A <see cref="IProxyFactory"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="CastleDynamicProxyFactory"/> (leveraging <see href="http://www.castleproject.org/projects/dynamicproxy/">Castle DynamicProxy</see>.).</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public IProxyFactory ProxyFactory
        {
            get
            {
                if (proxyFactory != null)
                {
                    return proxyFactory;
                }

                if (TrySingleInstanceFromServiceLocators(out proxyFactory))
                {
                    return proxyFactory;
                }

                proxyFactory = new CastleDynamicProxyFactory(Logger, MessageBroker, TimerStrategy, RuntimePolicyStrategy);
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
        /// Gets or sets the <see cref="ResourceEndpointConfiguration"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ResourceEndpointConfiguration"/>.
        /// </value>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ResourceEndpointConfiguration ResourceEndpoint
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
        /// <returns>A collection of <see cref="IResource"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IResource"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IResource> Resources
        {
            get
            {
                if (resources != null)
                {
                    return resources;
                }

                if (TryAllInstancesFromServiceLocators(out resources))
                {
                    return resources;
                }

                resources = CreateDiscoverableCollection<IResource>(XmlConfiguration.Resources);
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
        /// <returns>A collection of <see cref="IRuntimePolicy"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="IRuntimePolicy"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<IRuntimePolicy> RuntimePolicies
        {
            get
            {
                if (runtimePolicies != null)
                {
                    return runtimePolicies;
                }

                if (TryAllInstancesFromServiceLocators(out runtimePolicies))
                {
                    return runtimePolicies;
                }

                var collection = CreateDiscoverableCollection<IRuntimePolicy>(XmlConfiguration.RuntimePolicies);

                foreach (var config in collection.OfType<IConfigurable>())
                {
                    config.Configure(XmlConfiguration);
                }

                runtimePolicies = collection;
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
        /// Gets or sets the <see cref="RuntimePolicy"/> strategy.
        /// </summary>
        /// <value>
        /// The configured <see cref="RuntimePolicy"/>.
        /// </value>
        /// <returns>A <c>Func&lt;RuntimePolicy&gt;</c> to access the request specific <see cref="RuntimePolicy"/>.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public Func<RuntimePolicy> RuntimePolicyStrategy
        {
            get
            {
                if (runtimePolicyStrategy != null)
                {
                    return runtimePolicyStrategy;
                }
#warning CGIJBELS -> new implementation
                return () => GlimpseRuntime.Instance.CurrentRequestContext.ActiveRuntimePolicy; // TODO: Reimplement
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                runtimePolicyStrategy = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISerializer"/>.
        /// </summary>
        /// <value>
        /// The configured <see cref="ISerializer"/>.
        /// </value>
        /// <returns>A <see cref="ISerializer"/> instance resolved by the <see cref="IServiceLocator"/>s, otherwise <see cref="JsonNetSerializer"/> (leveraging <see href="http://json.codeplex.com/">Json.Net</see>).</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ISerializer Serializer
        {
            get
            {
                if (serializer != null)
                {
                    return serializer;
                }

                if (TrySingleInstanceFromServiceLocators(out serializer))
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
        /// <returns>A collection of <see cref="ISerializationConverter"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ISerializationConverter"/>s discovered in the configured discovery location.</returns>
        public ICollection<ISerializationConverter> SerializationConverters {
            get
            {
                if (serializationConverters != null)
                {
                    return serializationConverters;
                }

                if (TryAllInstancesFromServiceLocators(out serializationConverters))
                {
                    return serializationConverters;
                }

                serializationConverters = CreateDiscoverableCollection<ISerializationConverter>(XmlConfiguration.SerializationConverters);
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
        /// <returns>A collection of <see cref="ITab"/> instances resolved by the <see cref="IServiceLocator"/>s, otherwise all <see cref="ITab"/>s discovered in the configured discovery location.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public ICollection<ITab> Tabs
        {
            get
            {
                if (tabs != null)
                {
                    return tabs;
                }

                if (TryAllInstancesFromServiceLocators(out tabs))
                {
                    return tabs;
                }

                tabs = CreateDiscoverableCollection<ITab>(XmlConfiguration.Tabs);
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

        public ICollection<IDisplay> Displays
        {
            get
            {
                if (displays != null)
                {
                    return displays;
                }

                if (TryAllInstancesFromServiceLocators(out displays))
                {
                    return displays;
                }

                displays = CreateDiscoverableCollection<IDisplay>(XmlConfiguration.Displays);
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

        /// <summary>
        /// Gets or sets the <see cref="IExecutionTimer"/> strategy.
        /// </summary>
        /// <value>
        /// The configured <see cref="IExecutionTimer"/> strategy.
        /// </value>
        /// <returns>A <c>Func&lt;IExecutionTimer&gt;</c> to access the request specific <see cref="IExecutionTimer"/>.</returns>
        /// <exception cref="System.ArgumentNullException">An exception is thrown if the value is set to <c>null</c>.</exception>
        public Func<IExecutionTimer> TimerStrategy 
        { 
            get
            {
                if (timerStrategy != null)
                {
                    return timerStrategy;
                }

                return() => new ExecutionTimer(Stopwatch.StartNew()) as IExecutionTimer; // TODO: reimplement this
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                timerStrategy = value;
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

        private bool TrySingleInstanceFromServiceLocators<T>(out T instance) where T : class
        {
            if (UserServiceLocator != null)
            {
                instance = UserServiceLocator.GetInstance<T>();
                if (instance != null)
                {
                    return true;
                }
            }

            instance = null;
            return false;
        }

        private bool TryAllInstancesFromServiceLocators<T>(out ICollection<T> instance) where T : class
        {
            if (UserServiceLocator != null)
            {
                var result = UserServiceLocator.GetAllInstances<T>();
                if (result != null)
                {
                    instance = result;
                    return true;
                }
            }

            instance = null;
            return false;
        }

        private IDiscoverableCollection<T> CreateDiscoverableCollection<T>(DiscoverableCollectionElement config)
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<T>(Logger);

            discoverableCollection.IgnoredTypes.AddRange(ToEnumerable(config.IgnoredTypes));

            // config.DiscoveryLocation (collection specific) overrides Configuration.DiscoveryLocation (on main <glimpse> node)
            var locationCascade = string.IsNullOrEmpty(config.DiscoveryLocation)
                                       ? string.IsNullOrEmpty(XmlConfiguration.DiscoveryLocation)
                                             ? null
                                             : XmlConfiguration.DiscoveryLocation
                                       : config.DiscoveryLocation;

            if (locationCascade != null)
            {
                discoverableCollection.DiscoveryLocation = locationCascade;
            }

            discoverableCollection.AutoDiscover = config.AutoDiscover;
            if (discoverableCollection.AutoDiscover)
            {
                discoverableCollection.Discover();
            }

            return discoverableCollection;
        }

        private static IEnumerable<Type> ToEnumerable(TypeElementCollection collection)
        {
            foreach (TypeElement typeElement in collection)
            {
                yield return typeElement.Type;
            }
        }
    }
}
