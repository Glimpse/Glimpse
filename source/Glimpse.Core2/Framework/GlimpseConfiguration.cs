using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class GlimpseConfiguration : IGlimpseConfiguration
    {
        //TODO: Add Sanitizer?
        public GlimpseConfiguration(IFrameworkProvider frameworkProvider, 
            ResourceEndpointConfiguration endpointConfiguration,
            ICollection<IClientScript> clientScripts,
            ILogger logger,
            RuntimePolicy defaultRuntimePolicy,
            IHtmlEncoder htmlEncoder,
            IPersistanceStore persistanceStore,
            ICollection<IPipelineInspector> pipelineInspectors,
            ICollection<IResource> resources,
            ISerializer serializer,
            ICollection<ITab> tabs,
            ICollection<IRuntimePolicy> runtimePolicies,
            IResource defaultResource,
            IProxyFactory proxyFactory,
            IMessageBroker messageBroker)
        {
            //TODO: Test building glimpse on clean VS install with contracts
            //TODO: Test building glimpse on teamcity with contracts
            Contract.Requires<ArgumentNullException>(frameworkProvider != null, "frameworkProvider");
            Contract.Requires<ArgumentNullException>(endpointConfiguration != null, "endpointConfiguration");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(htmlEncoder != null, "htmlEncoder");
            Contract.Requires<ArgumentNullException>(persistanceStore != null, "persistanceStore");
            Contract.Requires<ArgumentNullException>(clientScripts != null, "clientScripts");
            Contract.Requires<ArgumentNullException>(pipelineInspectors != null, "pipelineInspectors");
            Contract.Requires<ArgumentNullException>(resources != null, "resources");
            Contract.Requires<ArgumentNullException>(serializer != null, "serializer");
            Contract.Requires<ArgumentNullException>(tabs != null, "tabs");
            Contract.Requires<ArgumentNullException>(runtimePolicies != null, "runtimePolicies");
            Contract.Requires<ArgumentNullException>(defaultResource != null, "defaultResource");
            Contract.Requires<ArgumentNullException>(proxyFactory != null, "proxyFactory");
            Contract.Requires<ArgumentNullException>(messageBroker != null, "messageBroker");

            Logger = logger;
            ClientScripts = clientScripts;
            FrameworkProvider = frameworkProvider;
            HtmlEncoder = htmlEncoder;
            PersistanceStore = persistanceStore;
            PipelineInspectors = pipelineInspectors;
            ResourceEndpoint = endpointConfiguration;
            Resources = resources;
            Serializer = serializer;
            Tabs = tabs;
            RuntimePolicies = runtimePolicies;
            DefaultRuntimePolicy = defaultRuntimePolicy;
            DefaultResource = defaultResource;
            ProxyFactory = proxyFactory;
            MessageBroker = messageBroker;
        }

        private ICollection<IClientScript> clientScripts;
        public ICollection<IClientScript> ClientScripts
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<IClientScript>>() != null);
                return clientScripts;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                clientScripts = value;
            }
        }

        private IFrameworkProvider frameworkProvider;
        public IFrameworkProvider FrameworkProvider
        {
            get
            {
                Contract.Ensures(Contract.Result<IFrameworkProvider>() != null);
                return frameworkProvider;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                frameworkProvider = value;
            }
        }

        private IHtmlEncoder htmlEncoder;
        public IHtmlEncoder HtmlEncoder
        {
            get
            {
                Contract.Ensures(Contract.Result<IHtmlEncoder>()!=null);
                return htmlEncoder;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                htmlEncoder = value;
            }
        }

        private ILogger logger;
        public ILogger Logger
        {
            get
            {
                Contract.Ensures(Contract.Result<ILogger>()!=null);
                return logger;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                logger = value;
            }
        }

        private IPersistanceStore persistanceStore;
        public IPersistanceStore PersistanceStore
        {
            get
            {
                Contract.Ensures(Contract.Result<IPersistanceStore>()!=null);
                return persistanceStore;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                persistanceStore = value;
            }
        }

        private ICollection<IPipelineInspector> pipelineInspectors;
        public ICollection<IPipelineInspector> PipelineInspectors
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<IPipelineInspector>>()!=null);
                return pipelineInspectors;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                pipelineInspectors = value;
            }
        }

        private ResourceEndpointConfiguration resourceEndpoint;
        public ResourceEndpointConfiguration ResourceEndpoint
        {
            get
            {
                Contract.Ensures(Contract.Result<ResourceEndpointConfiguration>()!=null);
                return resourceEndpoint;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                resourceEndpoint = value;
            }
        }

        private ICollection<IResource> resources;
        public ICollection<IResource> Resources
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<IResource>>()!=null);
                return resources;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                resources = value;
            }
        }

        private ISerializer serializer;
        public ISerializer Serializer
        {
            get
            {
                Contract.Ensures(Contract.Result<ISerializer>()!=null);
                return serializer;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                serializer = value;
            }
        }

        private ICollection<ITab> tabs;
        public ICollection<ITab> Tabs
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<ITab>>()!=null);
                return tabs;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                tabs = value;
            }
        }

        private ICollection<IRuntimePolicy> runtimePolicies;
        public ICollection<IRuntimePolicy> RuntimePolicies
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<IRuntimePolicy>>() != null);
                return runtimePolicies;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                runtimePolicies = value;
            }
        }

        private IResource defaultResource;
        public IResource DefaultResource
        {
            get
            {
                Contract.Ensures(Contract.Result<IResource>()!=null);
                return defaultResource;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                defaultResource = value;
            }
        }

        private IProxyFactory proxyFactory;
        public IProxyFactory ProxyFactory
        {
            get
            {
                Contract.Ensures(Contract.Result<IProxyFactory>()!=null);
                return proxyFactory;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                proxyFactory = value;
            }
        }

        private IMessageBroker messageBroker;
        public IMessageBroker MessageBroker
        {
            get
            {
                Contract.Ensures(Contract.Result<IMessageBroker>()!=null);
                return messageBroker;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                messageBroker = value;
            }
        }

        public RuntimePolicy DefaultRuntimePolicy { get; set; }
    }
}
