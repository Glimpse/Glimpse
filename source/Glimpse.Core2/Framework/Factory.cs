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

        public Factory():this(null){}

        public Factory(IServiceLocator providerServiceLocator):this(providerServiceLocator, null){}

        public Factory(IServiceLocator providerServiceLocator, IServiceLocator userServiceLocator)
        {
            //TODO: Try to lookup/load user service locator from config if null
            ProviderServiceLocator = providerServiceLocator;
            UserServiceLocator = userServiceLocator;
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseSection ?? new GlimpseSection();
        }

        public IGlimpseRuntime InstantiateRuntime()
        {
            Contract.Ensures(Contract.Result<IGlimpseRuntime>()!=null);

            IGlimpseRuntime result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            //TODO: Finish me!
            //Create a GlimpseConfiguration()
            throw new NotImplementedException();
        }

        private IFrameworkProvider FrameworkProvider { get; set; }
        public IFrameworkProvider InstantiateFrameworkProvider()
        {
            Contract.Ensures(Contract.Result<IFrameworkProvider>()!=null);

            if (FrameworkProvider != null) return FrameworkProvider;

            IFrameworkProvider result;
            if (TrySingleInstanceFromServiceLocators(out result))
            {
                FrameworkProvider = result;
                return FrameworkProvider;
            }

            //TODO: Turn this string into a resource and provide better information
            throw new GlimpseException("Unable to create Framework Provider.");
        }

        public ResourceEndpointConfiguration InstantiateEndpointConfiguration()
        {
            Contract.Ensures(Contract.Result<ResourceEndpointConfiguration>()!=null);

            ResourceEndpointConfiguration result;
            if (TrySingleInstanceFromServiceLocators(out result)) return result;

            //TODO: Turn this string into a resource and provide better information
            throw new GlimpseException("Unable to create Endpoint Configuration.");
        }

        public ICollection<IClientScript> InstantiateClientScripts()
        {
            Contract.Ensures(Contract.Result<ICollection<IClientScript>>()!=null);

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
                                     Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}|${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method}"
                                 };

            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget("file", fileTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.FromOrdinal((int)logLevel), fileTarget));


            Logger = new NLogLogger(new LogFactory(loggingConfiguration).GetLogger("Glimpse"));
            return Logger;
        }

        public RuntimePolicy InstantiateBaseRuntimePolicy()
        {
            return Configuration.BaseRuntimePolicy;
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
            Contract.Ensures(Contract.Result<ICollection<IPipelineInspector>>()!=null);

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

            return new JsonNetSerializer();
        }

        public ICollection<ITab> InstantiateTabs()
        {
            Contract.Ensures(Contract.Result<ICollection<ITab>>()!=null);

            ICollection<ITab> tabs;
            if (TryAllInstancesFromServiceLocators(out tabs)) return tabs;

            return CreateDiscoverableCollection<ITab>(Configuration.Tabs);
        }

        public ICollection<IRuntimePolicy> InstantiateRuntimePolicies()
        {
            Contract.Ensures(Contract.Result<ICollection<IRuntimePolicy>>()!=null);

            ICollection<IRuntimePolicy> policies;
            if (TryAllInstancesFromServiceLocators(out policies)) return policies;

            return CreateDiscoverableCollection<IRuntimePolicy>(Configuration.RuntimePolicies);
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

        private bool TrySingleInstanceFromServiceLocators<T>(out T instance) where T: class
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