using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Policy;
using Glimpse.Core.Resource;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// The main bootstrapper for Glimpse, <c>Factory</c> (or its derived types) is responsible for instantiating all required configurable types.
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class without any <see cref="IServiceLocator"/> implementations.
        /// </summary>
        public Factory() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class without a <see cref="IServiceLocator"/> implementation from the framework provider.
        /// </summary>
        /// <param name="providerServiceLocator">The framework provider's service locator.</param>
        public Factory(IServiceLocator providerServiceLocator) : this(providerServiceLocator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        /// <param name="providerServiceLocator">The framework provider's service locator.</param>
        /// <param name="userServiceLocator">The user's service locator.</param>
        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator) : this(providerServiceLocator, userServiceLocator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        /// <param name="providerServiceLocator">The framework provider's service locator.</param>
        /// <param name="userServiceLocator">The user's service locator.</param>
        /// <param name="configuration">The Glimpse configuration to use.</param>
        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator, Section configuration)
        {
            Configuration = configuration ?? ConfigurationManager.GetSection("glimpse") as Section ?? new Section();

            IServiceLocator loadedServiceLocator = null;
            if (userServiceLocator == null && Configuration.ServiceLocatorType != null)
            {
                loadedServiceLocator = Activator.CreateInstance(Configuration.ServiceLocatorType) as IServiceLocator;
            }

            ProviderServiceLocator = providerServiceLocator;
            UserServiceLocator = userServiceLocator ?? loadedServiceLocator;
        }

        internal IServiceLocator UserServiceLocator { get; set; }

        internal IServiceLocator ProviderServiceLocator { get; set; }
        
        internal Section Configuration { get; set; }

        private ILogger Logger { get; set; }

        private IFrameworkProvider FrameworkProvider { get; set; }

        private IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Instantiates an instance of <see cref="IGlimpseRuntime"/>.
        /// </summary>
        /// <returns>A <see cref="IGlimpseRuntime"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="GlimpseRuntime"/>.</returns>
        public IGlimpseRuntime InstantiateRuntime()
        {
            IGlimpseRuntime result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                return result;
            }

            return new GlimpseRuntime(InstantiateConfiguration());
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IFrameworkProvider"/>.
        /// </summary>
        /// <returns>A <see cref="IFrameworkProvider"/> instance resolved by one of the <see cref="IServiceLocator"/>s.</returns>
        /// <exception cref="GlimpseException">An exception is thrown is an instance of <see cref="IFrameworkProvider"/> is not provided by a <see cref="IServiceLocator"/>.</exception>
        public IFrameworkProvider InstantiateFrameworkProvider()
        {
            if (FrameworkProvider != null)
            {
                return FrameworkProvider;
            }

            IFrameworkProvider result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                FrameworkProvider = result;
                return FrameworkProvider;
            }

            throw new GlimpseException(
                string.Format(
                    Resources.InstantiateFrameworkProviderException,
                    UserServiceLocator == null ? "UserServiceLocator not configured" : UserServiceLocator.GetType().AssemblyQualifiedName,
                    ProviderServiceLocator == null ? "ProviderServiceLocator not configured" : ProviderServiceLocator.GetType().AssemblyQualifiedName));
        }

        /// <summary>
        /// Instantiates an instance of <see cref="ResourceEndpointConfiguration"/>.
        /// </summary>
        /// <returns>A <see cref="ResourceEndpointConfiguration"/> instance resolved by one of the <see cref="IServiceLocator"/>s.</returns>
        /// <exception cref="GlimpseException">An exception is thrown is an instance of <see cref="ResourceEndpointConfiguration"/> is not provided by a <see cref="IServiceLocator"/>.</exception>
        public ResourceEndpointConfiguration InstantiateResourceEndpointConfiguration()
        {
            ResourceEndpointConfiguration result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                return result;
            }

            throw new GlimpseException(
                string.Format(
                    Resources.InstantiateResourceEndpointConfigurationException,
                    UserServiceLocator == null ? "UserServiceLocator not configured" : UserServiceLocator.GetType().AssemblyQualifiedName,
                    ProviderServiceLocator == null ? "ProviderServiceLocator not configured" : ProviderServiceLocator.GetType().AssemblyQualifiedName));
        }

        /// <summary>
        /// Instantiates a collection of <see cref="IClientScript"/>s.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="IClientScript"/> instances resolved by one of the <see cref="IServiceLocator"/>s, otherwise all <see cref="IClientScript"/>s discovered in the configured discovery location.
        /// </returns>
        public ICollection<IClientScript> InstantiateClientScripts()
        {
            return InstantiateDiscoverableCollection<IClientScript>(Configuration.ClientScripts);
        }

        /// <summary>
        /// Instantiates an instance of <see cref="ILogger"/>.
        /// </summary>
        /// <returns>A <see cref="ILogger"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise a <see cref="NullLogger"/> or <see cref="NLogLogger"/> (leveraging the <see href="http://nlog-project.org/">NLog</see> project) based on configuration settings.</returns>
        public ILogger InstantiateLogger()
        {
            // reuse logger if already created
            if (Logger != null)
            {
                return Logger;
            }

            ILogger result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                Logger = result;
                return Logger;
            }

            // use null logger if logging is off
            var logLevel = Configuration.Logging.Level;
            if (logLevel == LoggingLevel.Off)
            {
                Logger = new NullLogger();
                return Logger;
            }

            var configuredPath = Configuration.Logging.LogLocation;
            
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

            Logger = new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
            return Logger;
        }

        /// <summary>
        /// Instantiates the default instance of <see cref="RuntimePolicy"/>.
        /// </summary>
        /// <returns>A <see cref="RuntimePolicy"/> instance based on configuration settings.</returns>
        public RuntimePolicy InstantiateDefaultRuntimePolicy()
        {
            return Configuration.DefaultRuntimePolicy;
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IHtmlEncoder"/>.
        /// </summary>
        /// <returns>A <see cref="IHtmlEncoder"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="AntiXssEncoder"/> (leveraging the <see href="http://wpl.codeplex.com/">Microsoft Web Protection Library</see>).</returns>
        public IHtmlEncoder InstantiateHtmlEncoder()
        {
            IHtmlEncoder encoder;

            if (TrySingleInstanceFromServiceLocators(out encoder))
            {
                return encoder;
            }

            return new AntiXssEncoder();
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IPersistenceStore"/>.
        /// </summary>
        /// <returns>A <see cref="IPersistenceStore"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="ApplicationPersistenceStore"/>.</returns>
        public IPersistenceStore InstantiatePersistenceStore()
        {
            IPersistenceStore store;
            if (TrySingleInstanceFromServiceLocators(out store))
            {
                return store;
            }

            return new ApplicationPersistenceStore(InstantiateFrameworkProvider().HttpServerStore);
        }

        /// <summary>
        /// Instantiates a collection of <see cref="IInspector"/>s.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="IInspector"/> instances resolved by one of the <see cref="IServiceLocator"/>s, otherwise all <see cref="IInspector"/>s discovered in the configured discovery location.
        /// </returns>
        public ICollection<IInspector> InstantiateInspectors()
        {
            return InstantiateDiscoverableCollection<IInspector>(Configuration.Inspectors);
        }

        /// <summary>
        /// Instantiates a collection of <see cref="IResource"/>s.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="IResource"/> instances resolved by one of the <see cref="IServiceLocator"/>s, otherwise all <see cref="IResource"/>s discovered in the configured discovery location.
        /// </returns>
        public ICollection<IResource> InstantiateResources()
        {
            return InstantiateDiscoverableCollection<IResource>(Configuration.Resources);
        }

        /// <summary>
        /// Instantiates an instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <returns>A <see cref="ISerializer"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="JsonNetSerializer"/> (leveraging <see href="http://json.codeplex.com/">Json.Net</see>).</returns>
        public ISerializer InstantiateSerializer()
        {
            ISerializer result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                return result;
            }

            result = new JsonNetSerializer(InstantiateLogger());
            result.RegisterSerializationConverters(InstantiateSerializationConverters());

            return result;
        }

        /// <summary>
        /// Instantiates a collection of <see cref="ITab"/>s.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="ITab"/> instances resolved by one of the <see cref="IServiceLocator"/>s, otherwise all <see cref="ITab"/>s discovered in the configured discovery location.
        /// </returns>
        public ICollection<ITab> InstantiateTabs()
        {
            return InstantiateDiscoverableCollection<ITab>(Configuration.Tabs);
        }

        public ICollection<IDisplay> InstantiateDisplays()
        {
            return InstantiateDiscoverableCollection<IDisplay>(Configuration.Displays);
        }

        /// <summary>
        /// Instantiates a collection of <see cref="IRuntimePolicy"/>s.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="IRuntimePolicy"/> instances resolved by one of the <see cref="IServiceLocator"/>s, otherwise all <see cref="IRuntimePolicy"/>s discovered in the configured discovery location.
        /// </returns>
        public ICollection<IRuntimePolicy> InstantiateRuntimePolicies()
        {
            return InstantiateDiscoverableCollection<IRuntimePolicy>(Configuration.RuntimePolicies, runtimePolicies =>
            {

#warning begin of backward compatibility hack that should be removed in v2

                Action<IContentTypePolicyConfigurator> contentTypePolicyBackwardHack = configurator =>
                {
                    if (configurator == null)
                    {
                        return;
                    }

                    foreach (var supportedContentType in configurator.SupportedContentTypes)
                    {
                        Configuration.RuntimePolicies.ContentTypes.Add(new ContentTypeElement { ContentType = supportedContentType.ContentType });
                    }
                };

                Action<IStatusCodePolicyConfigurator> statusCodePolicyBackwardHack = configurator =>
                {
                    if (configurator == null)
                    {
                        return;
                    }

                    foreach (var supportedStatusCode in configurator.SupportedStatusCodes)
                    {
                        Configuration.RuntimePolicies.StatusCodes.Add(new StatusCodeElement { StatusCode = supportedStatusCode });
                    }
                };

                Action<IUriPolicyConfigurator> uriPolicyBackwardHack = configurator =>
                {
                    if (configurator == null)
                    {
                        return;
                    }

                    foreach (var uriPatternsToIgnore in configurator.UriPatternsToIgnore)
                    {
                        Configuration.RuntimePolicies.Uris.Add(new RegexElement { Regex = uriPatternsToIgnore });
                    }
                };

                foreach (var runtimePolicy in runtimePolicies)
                {
                    var runtimePolicyAsConfigurableExtended = runtimePolicy as IConfigurableExtended;
                    if (runtimePolicyAsConfigurableExtended != null)
                    {
                        contentTypePolicyBackwardHack(runtimePolicyAsConfigurableExtended.Configurator as IContentTypePolicyConfigurator);
                        statusCodePolicyBackwardHack(runtimePolicyAsConfigurableExtended.Configurator as IStatusCodePolicyConfigurator);
                        uriPolicyBackwardHack(runtimePolicyAsConfigurableExtended.Configurator as IUriPolicyConfigurator);
                    }
                }

#warning end of backward compatibility hack that should be removed in v2
            });
        }

        /// <summary>
        /// Instantiates a collection of <see cref="ISerializationConverter"/>s.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="ISerializationConverter"/> instances resolved by one of the <see cref="IServiceLocator"/>s, otherwise all <see cref="ISerializationConverter"/>s discovered in the configured discovery location.
        /// </returns>
        public ICollection<ISerializationConverter> InstantiateSerializationConverters()
        {
            return InstantiateDiscoverableCollection<ISerializationConverter>(Configuration.SerializationConverters);
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IResource"/>.
        /// </summary>
        /// <returns>A <see cref="IResource"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="ConfigurationResource"/>.</returns>
        public IResource InstantiateDefaultResource()
        {
            IResource result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                return result;
            }

            return new ConfigurationResource();
        }

        /// <summary>
        /// Instantiates a strategy pattern for accessing an instance of <see cref="IExecutionTimer"/>.
        /// </summary>
        /// <returns>
        /// A <c>Func&lt;IExecutionTimer&gt;</c> to access the request specific <see cref="IExecutionTimer"/>.
        /// </returns>
        public Func<IExecutionTimer> InstantiateTimerStrategy()
        {
            var frameworkProvider = InstantiateFrameworkProvider();

            return () => frameworkProvider.HttpRequestStore.Get<IExecutionTimer>(Constants.GlobalTimerKey);
        }

        /// <summary>
        /// Instantiates a strategy pattern for accessing an instance of <see cref="RuntimePolicy"/>.
        /// </summary>
        /// <returns>
        /// A <c>Func&lt;RuntimePolicy&gt;</c> to access the request specific <see cref="RuntimePolicy"/>.
        /// </returns>
        public Func<RuntimePolicy> InstantiateRuntimePolicyStrategy()
        {
            var frameworkProvider = InstantiateFrameworkProvider();
            var logger = InstantiateLogger();

            return () =>
            {
                // this code is indirectly called from 2 places :
                // - From inside an AlternateMethod instance (or basically everything that is related to a Glimpse proxy) to decide whether 
                //   or not Glimpse is enabled and data should be collected, and in case RuntimePolicy.Off is returned, the original method 
                //   will be called, which has the same effect as if Glimpse is not there.
                // - By any Inspector, since it is exposed on the InspectorContext

                // Now the assumption that is made here, is that this code will only be called after that the GlimpseRuntime's BeginRequest method
                // has run and properly initialized the 'GlimpseContext' for the current request, which means it has at least set the current runtime policy. 
                // Unfortunately there are use-cases where users are creative and (ab)use specific concepts to achieve a specific goal, and those uses don't
                // always align with Glimpse's assumptions. For example a new instance of an HttpContext is sometimes created and assigned to the HttpContext.Current
                // property to have a new controller instance render a view to a string as if it was a request... This has the nasty side-effect that Glimpse is not
                // given the opportunity to do a proper setup of that request, resulting in non-deterministic behavior.
                
                // Therefore if we notice that the current request has not properly been initialized by the GlimpseRuntime's BeginRequest method then we'll decide
                // that Glimpse is disabled, which is the safest assumption we can make here, preventing any further Glimpse specific code from collection information for that new "request".
                if (!frameworkProvider.HttpRequestStore.Contains(Constants.RuntimePolicyKey))
                {
                    logger.Debug("Apparently GlimpseRuntime has not yet initialized this request. This might happen in case you're doing something specific like mentioned in this issue: https://github.com/Glimpse/Glimpse/issues/703 . Either way, Glimpse will be disabled to prevent any further non-deterministic behavior during this request.");
                    // we'll store a RuntimePolicy.Off in the HttpRequestStore for subsequent calls for this request.
                    frameworkProvider.HttpRequestStore.Set(Constants.RuntimePolicyKey, RuntimePolicy.Off);
                }

                return frameworkProvider.HttpRequestStore.Get<RuntimePolicy>(Constants.RuntimePolicyKey);
            };
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IGlimpseConfiguration"/>.
        /// </summary>
        /// <returns>A <see cref="IGlimpseConfiguration"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="GlimpseConfiguration"/> with each constructor parameter created with the corresponding <see cref="Factory"/> method.</returns>
        public IGlimpseConfiguration InstantiateConfiguration()
        {
            IGlimpseConfiguration result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                return result;
            }

            var frameworkProvider = InstantiateFrameworkProvider();
            var timerStrategy = InstantiateTimerStrategy();
            var runtimePolicyStrategy = InstantiateRuntimePolicyStrategy();
            var endpointConfiguration = InstantiateResourceEndpointConfiguration();
            var logger = InstantiateLogger();
            var runtimePolicies = InstantiateRuntimePolicies();
            var clientScripts = InstantiateClientScripts();
            var policy = InstantiateDefaultRuntimePolicy();
            var htmlEncoder = InstantiateHtmlEncoder();
            var persistenceStore = InstantiatePersistenceStore();
            var inspectors = InstantiateInspectors();
            var resources = InstantiateResources();
            var serializer = InstantiateSerializer();
            var tabs = InstantiateTabs();
            var displays = InstantiateDisplays();
            var defaultResource = InstantiateDefaultResource();
            var proxyFactory = InstantiateProxyFactory();
            var messageBroker = InstantiateMessageBroker();
            var endpointBaseUri = InstantiateBaseResourceUri();

            return new GlimpseConfiguration(frameworkProvider, endpointConfiguration, clientScripts, logger, policy, htmlEncoder, persistenceStore, inspectors, resources, serializer, tabs, displays, runtimePolicies, defaultResource, proxyFactory, messageBroker, endpointBaseUri, timerStrategy, runtimePolicyStrategy);
        }

        /// <summary>
        /// Instantiates a string that represents the base Uri Glimpse will use for invoking all instances of <see cref="IResource"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> instance based on configuration settings.</returns>
        public string InstantiateBaseResourceUri()
        {
            return Configuration.EndpointBaseUri;
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IMessageBroker"/>.
        /// </summary>
        /// <returns>A <see cref="IMessageBroker"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="MessageBroker"/>.</returns>
        public IMessageBroker InstantiateMessageBroker()
        {
            if (MessageBroker == null)
            { 
                IMessageBroker result;
                if (TrySingleInstanceFromServiceLocators(out result))
                {
                    MessageBroker = result; 
                }
                else
                {
                    MessageBroker = new MessageBroker(InstantiateLogger());
                }
            }

            return MessageBroker;
        }

        /// <summary>
        /// Instantiates an instance of <see cref="IProxyFactory"/>.
        /// </summary>
        /// <returns>A <see cref="IProxyFactory"/> instance resolved by one of the <see cref="IServiceLocator"/>s, otherwise <see cref="CastleDynamicProxyFactory"/> (leveraging <see href="http://www.castleproject.org/projects/dynamicproxy/">Castle DynamicProxy</see>.).</returns>
        public IProxyFactory InstantiateProxyFactory()
        {
            IProxyFactory result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                return result;
            }

            return new CastleDynamicProxyFactory(InstantiateLogger(), InstantiateMessageBroker(), InstantiateTimerStrategy(), InstantiateRuntimePolicyStrategy());
        }

        private IDiscoverableCollection<T> CreateDiscoverableCollection<T>(DiscoverableCollectionElement config)
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<T>(InstantiateLogger());

            discoverableCollection.IgnoredTypes.AddRange(config.IgnoredTypes);

            // config.DiscoveryLocation (collection specific) overrides Configuration.DiscoveryLocation (on main <glimpse> node)
            var locationCascade = string.IsNullOrEmpty(config.DiscoveryLocation)
                                       ? string.IsNullOrEmpty(Configuration.DiscoveryLocation)
                                             ? null
                                             : Configuration.DiscoveryLocation
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

        private ICollection<TElementType> InstantiateDiscoverableCollection<TElementType>(DiscoverableCollectionElement configuredDiscoverableCollection, Action<ICollection<TElementType>> onBeforeConfiguringElements = null)
            where TElementType : class
        {
            ICollection<TElementType> collection;
            if (TryAllInstancesFromServiceLocators(out collection))
            {
                return collection;
            }

            var discoverableCollection = CreateDiscoverableCollection<TElementType>(configuredDiscoverableCollection);

            // get the list of configurators
            var configurators = discoverableCollection.OfType<IConfigurableExtended>()
                .Select(configurable => configurable.Configurator)
                .GroupBy(configurator=> configurator.CustomConfigurationKey);

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

            if (onBeforeConfiguringElements != null)
            {
                onBeforeConfiguringElements(discoverableCollection);
            }

            foreach (var configurable in discoverableCollection.OfType<IConfigurable>())
            {
                configurable.Configure(Configuration);
            }

            return discoverableCollection;
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

            if (ProviderServiceLocator != null)
            {
                instance = ProviderServiceLocator.GetInstance<T>();
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
            IEnumerable<T> result;
            if (UserServiceLocator != null)
            {
                result = UserServiceLocator.GetAllInstances<T>();
                if (result != null)
                {
                    instance = result as IList<T>;
                    return true;
                }
            }

            if (ProviderServiceLocator != null)
            {
                result = ProviderServiceLocator.GetAllInstances<T>();
                if (result != null)
                {
                    instance = result as IList<T>;
                    return true;
                }
            }

            instance = null;
            return false;
        }
    }
}