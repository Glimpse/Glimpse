using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Configuration;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Extensions;
using NLog;
using NLog.Config;
using NLog.Targets;

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

        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator,
                       GlimpseSection configuration)
        {
            Configuration = configuration ??
                            ConfigurationManager.GetSection("glimpse") as GlimpseSection ?? new GlimpseSection();

            IServiceLocator loadedServiceLocator = null;
            if (userServiceLocator == null && Configuration.ServiceLocatorType != null)
                loadedServiceLocator = Activator.CreateInstance(Configuration.ServiceLocatorType) as IServiceLocator;

            ProviderServiceLocator = providerServiceLocator;
            UserServiceLocator = userServiceLocator ?? loadedServiceLocator;
        }

        public IGlimpseRuntime InstantiateRuntime()
        {
            Contract.Ensures(Contract.Result<IGlimpseRuntime>() != null);

            IGlimpseRuntime result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new GlimpseRuntime(InstantiateConfiguration());
        }

        private IFrameworkProvider FrameworkProvider { get; set; }

        public IFrameworkProvider InstantiateFrameworkProvider()
        {
            Contract.Ensures(Contract.Result<IFrameworkProvider>() != null);

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
            Contract.Ensures(Contract.Result<ResourceEndpointConfiguration>() != null);

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
            Contract.Ensures(Contract.Result<ICollection<IClientScript>>() != null);

            ICollection<IClientScript> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<IClientScript>(Configuration.ClientScripts);
        }

        private ILogger Logger { get; set; }

        public ILogger InstantiateLogger()
        {
            Contract.Ensures(Contract.Result<ILogger>() != null);

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

            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget("file", fileTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int) logLevel), fileTarget));


            Logger = new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
            return Logger;
        }

        public RuntimePolicy InstantiateDefaultRuntimePolicy()
        {
            return Configuration.DefaultRuntimePolicy;
        }

        public IHtmlEncoder InstantiateHtmlEncoder()
        {
            Contract.Ensures(Contract.Result<IHtmlEncoder>() != null);

            IHtmlEncoder encoder;

            if (TrySingleInstanceFromServiceLocators(out encoder)) return encoder;

            return new AntiXssEncoder();
        }

        public IPersistanceStore InstantiatePersistanceStore()
        {
            Contract.Ensures(Contract.Result<IPersistanceStore>() != null);

            IPersistanceStore store;
            if (TrySingleInstanceFromServiceLocators(out store)) return store;

            return new ApplicationPersistanceStore(InstantiateFrameworkProvider().HttpServerStore);
        }

        public ICollection<IPipelineInspector> InstantiatePipelineInspectors()
        {
            Contract.Ensures(Contract.Result<ICollection<IPipelineInspector>>() != null);

            ICollection<IPipelineInspector> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<IPipelineInspector>(Configuration.PipelineInspectors);
        }

        public ICollection<IResource> InstantiateResources()
        {
            Contract.Ensures(Contract.Result<ICollection<IResource>>() != null);

            ICollection<IResource> resources;
            if (TryAllInstancesFromServiceLocators(out resources)) return resources;

            return CreateDiscoverableCollection<IResource>(Configuration.Resources);
        }

        public ISerializer InstantiateSerializer()
        {
            Contract.Ensures(Contract.Result<ISerializer>() != null);

            ISerializer result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            result = new JsonNetSerializer(InstantiateLogger());
            result.RegisterSerializationConverters(InstantiateSerializationConverters());

            return result;
        }

        public ICollection<ITab> InstantiateTabs()
        {
            Contract.Ensures(Contract.Result<ICollection<ITab>>() != null);

            ICollection<ITab> tabs;
            if (TryAllInstancesFromServiceLocators(out tabs)) return tabs;

            return CreateDiscoverableCollection<ITab>(Configuration.Tabs);
        }

        public ICollection<IRuntimePolicy> InstantiateRuntimePolicies()
        {
            Contract.Ensures(Contract.Result<ICollection<IRuntimePolicy>>() != null);

            ICollection<IRuntimePolicy> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<IRuntimePolicy>(Configuration.RuntimePolicies);
            //TODO: Special configuration for Uri, StatusCode and ContentType policies
        }

        public ICollection<ISerializationConverter> InstantiateSerializationConverters()
        {
            Contract.Ensures(Contract.Result<ICollection<ISerializationConverter>>() != null);

            ICollection<ISerializationConverter> result;
            if (TryAllInstancesFromServiceLocators(out result)) return result;

            return CreateDiscoverableCollection<ISerializationConverter>(Configuration.SerializationConverters);
        }

        public IResource InstantiateDefaultResource()
        {
            Contract.Ensures(Contract.Result<IResource>() != null);

            IResource result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new Resource.Configuration();
        }

        public IGlimpseConfiguration InstantiateConfiguration()
        {
            Contract.Ensures(Contract.Result<IGlimpseConfiguration>() != null);

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

            return new GlimpseConfiguration(frameworkProvider, endpointConfiguration, clientScripts, logger, policy,
                                            htmlEncoder, persistanceStore, pipelineInspectors, resources, serializer,
                                            tabs,
                                            runtimePolicies, defaultResource, proxyFactory, messageBroker);
        }

        public IMessageBroker InstantiateMessageBroker()
        {
            Contract.Ensures(Contract.Result<IMessageBroker>()!=null);

            IMessageBroker result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            return new MessageBroker(InstantiateLogger());
        }

        public IProxyFactory InstantiateProxyFactory()
        {
            Contract.Ensures(Contract.Result<IProxyFactory>() != null);

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
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out instance) != null);

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
            Contract.Ensures(!Contract.Result<bool>() || Contract.ValueAtReturn(out instance) != null);

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