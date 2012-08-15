using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Glimpse.Core2.Configuration;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Extensions;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Glimpse.Core2.Framework
{
    public class Factory
    {
        internal IServiceLocator UserServiceLocator { get; set; }
        internal IServiceLocator ProviderServiceLocator { get; set; }
        internal GlimpseSection Configuration { get; set; }

        public Factory() : this(null)
        {
        }

        public Factory(IServiceLocator providerServiceLocator) : this(providerServiceLocator, null)
        {
        }

        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator)
            : this(providerServiceLocator, userServiceLocator, null)
        {
        }

        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator, GlimpseSection configuration)
        {
            Configuration = configuration ?? ConfigurationManager.GetSection("glimpse") as GlimpseSection ?? new GlimpseSection();

            IServiceLocator loadedServiceLocator = null;
            if (userServiceLocator == null && Configuration.ServiceLocatorType != null)
                loadedServiceLocator = Activator.CreateInstance(Configuration.ServiceLocatorType) as IServiceLocator;

            ProviderServiceLocator = providerServiceLocator;
            UserServiceLocator = userServiceLocator ?? loadedServiceLocator;
        }

        public IGlimpseRuntime InstantiateRuntime()
        {
            IGlimpseRuntime result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new GlimpseRuntime(InstantiateConfiguration());
        }

        private IFrameworkProvider FrameworkProvider { get; set; }

        public IFrameworkProvider InstantiateFrameworkProvider()
        {
            if (FrameworkProvider != null) return FrameworkProvider;

            IFrameworkProvider result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                FrameworkProvider = result;
                return FrameworkProvider;
            }

            throw new GlimpseException(string.Format(Resources.InstantiateFrameworkProviderException,
                                                     UserServiceLocator == null
                                                         ? "UserServiceLocator not configured"
                                                         : UserServiceLocator.GetType().AssemblyQualifiedName,
                                                     ProviderServiceLocator == null
                                                         ? "ProviderServiceLocator not configured"
                                                         : ProviderServiceLocator.GetType().AssemblyQualifiedName));
        }

        public ResourceEndpointConfiguration InstantiateResourceEndpointConfiguration()
        {
            ResourceEndpointConfiguration result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;


            throw new GlimpseException(
                string.Format(Resources.InstantiateResourceEndpointConfigurationException,
                              UserServiceLocator == null
                                  ? "UserServiceLocator not configured"
                                  : UserServiceLocator.GetType().AssemblyQualifiedName,
                              ProviderServiceLocator == null
                                  ? "ProviderServiceLocator not configured"
                                  : ProviderServiceLocator.GetType().AssemblyQualifiedName));
        }

        public ICollection<IClientScript> InstantiateClientScripts()
        {
            ICollection<IClientScript> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<IClientScript>(Configuration.ClientScripts);
        }

        private ILogger Logger { get; set; }

        public ILogger InstantiateLogger()
        {
            //reuse logger if already created
            if (Logger != null) return Logger;

            ILogger result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                Logger = result;
                return Logger;
            }

            //use null logger if logging is off
            var logLevel = Configuration.Logging.Level;
            if (logLevel == LoggingLevel.Off)
            {
                Logger = new NullLogger();
                return Logger;
            }


            //use NLog logger otherwise
            var fileTarget = new FileTarget
                                 {
                                     FileName = "${basedir}/Glimpse.log",
                                     Layout =
                                         "${longdate} | ${level:uppercase=true} | ${message} | ${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method:innerExceptionSeparator=>>}"
                                 };

            var asyncTarget = new AsyncTargetWrapper(fileTarget);

            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget("file", asyncTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int) logLevel), asyncTarget));


            Logger = new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
            return Logger;
        }

        public RuntimePolicy InstantiateDefaultRuntimePolicy()
        {
            return Configuration.DefaultRuntimePolicy;
        }

        public IHtmlEncoder InstantiateHtmlEncoder()
        {
            IHtmlEncoder encoder;

            if (TrySingleInstanceFromServiceLocators(out encoder)) return encoder;

            return new AntiXssEncoder();
        }

        public IPersistanceStore InstantiatePersistanceStore()
        {
            IPersistanceStore store;
            if (TrySingleInstanceFromServiceLocators(out store)) return store;

            return new ApplicationPersistanceStore(InstantiateFrameworkProvider().HttpServerStore);
        }

        public ICollection<IPipelineInspector> InstantiatePipelineInspectors()
        {
            ICollection<IPipelineInspector> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<IPipelineInspector>(Configuration.PipelineInspectors);
        }

        public ICollection<IResource> InstantiateResources()
        {
            ICollection<IResource> resources;
            if (TryAllInstancesFromServiceLocators(out resources)) return resources;

            return CreateDiscoverableCollection<IResource>(Configuration.Resources);
        }

        public ISerializer InstantiateSerializer()
        {
            ISerializer result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            result = new JsonNetSerializer(InstantiateLogger());
            result.RegisterSerializationConverters(InstantiateSerializationConverters());

            return result;
        }

        public ICollection<ITab> InstantiateTabs()
        {
            ICollection<ITab> tabs;
            if (TryAllInstancesFromServiceLocators(out tabs)) return tabs;

            return CreateDiscoverableCollection<ITab>(Configuration.Tabs);
        }

        public ICollection<IRuntimePolicy> InstantiateRuntimePolicies()
        {
            ICollection<IRuntimePolicy> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            var collection = CreateDiscoverableCollection<IRuntimePolicy>(Configuration.RuntimePolicies);

            foreach (var config in collection.OfType<IConfigurable>())
            {
                config.Configure(Configuration);
            }

            return collection;
        }

        public ICollection<ISerializationConverter> InstantiateSerializationConverters()
        {
            ICollection<ISerializationConverter> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<ISerializationConverter>(Configuration.SerializationConverters);
        }

        public IResource InstantiateDefaultResource()
        {
            IResource result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new Resource.ConfigurationResource();
        }

        public IGlimpseConfiguration InstantiateConfiguration()
        {
            IGlimpseConfiguration result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            var frameworkProvider = InstantiateFrameworkProvider();
            var endpointConfiguration = InstantiateResourceEndpointConfiguration();
            var clientScripts = InstantiateClientScripts();
            var logger = InstantiateLogger();
            var policy = InstantiateDefaultRuntimePolicy();
            var htmlEncoder = InstantiateHtmlEncoder();
            var persistanceStore = InstantiatePersistanceStore();
            var pipelineInspectors = InstantiatePipelineInspectors();
            var resources = InstantiateResources();
            var serializer = InstantiateSerializer();
            var tabs = InstantiateTabs();
            var runtimePolicies = InstantiateRuntimePolicies();
            var defaultResource = InstantiateDefaultResource();
            var proxyFactory = InstantiateProxyFactory();
            var messageBroker = InstantiateMessageBroker();
            var endpointBaseUri = InstantiateBaseResourceUri();

            return new GlimpseConfiguration(frameworkProvider, endpointConfiguration, clientScripts, logger, policy,
                                            htmlEncoder, persistanceStore, pipelineInspectors, resources, serializer,
                                            tabs,
                                            runtimePolicies, defaultResource, proxyFactory, messageBroker, endpointBaseUri);
        }

        public string InstantiateBaseResourceUri()
        {
            return Configuration.EndpointBaseUri;
        }

        public IMessageBroker InstantiateMessageBroker()
        {
            IMessageBroker result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new MessageBroker(InstantiateLogger());
        }

        public IProxyFactory InstantiateProxyFactory()
        {
            IProxyFactory result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new CastleDynamicProxyFactory(InstantiateLogger());
        }

        private IDiscoverableCollection<T> CreateDiscoverableCollection<T>(DiscoverableCollectionElement config)
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<T>(InstantiateLogger());

            discoverableCollection.IgnoredTypes.AddRange(config.IgnoredTypes.ToEnumerable());

            if (config.DiscoveryLocation != DiscoverableCollectionElement.DefaultLocation)
                discoverableCollection.DiscoveryLocation = config.DiscoveryLocation;

            discoverableCollection.AutoDiscover = config.AutoDiscover;
            if (discoverableCollection.AutoDiscover) discoverableCollection.Discover();

            return discoverableCollection;
        }

        private bool TrySingleInstanceFromServiceLocators<T>(out T instance) where T : class
        {
            if (UserServiceLocator != null)
            {
                instance = UserServiceLocator.GetInstance<T>();
                if (instance != null) return true;
            }

            if (ProviderServiceLocator != null)
            {
                instance = ProviderServiceLocator.GetInstance<T>();
                if (instance != null) return true;
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