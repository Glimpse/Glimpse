using System;
using System.Collections.Generic;
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
            IMessageBroker messageBroker,
            string endpointBaseUri)
        {
            if (frameworkProvider == null) throw new ArgumentNullException("frameworkProvider");
            if (endpointConfiguration == null) throw new ArgumentNullException("endpointConfiguration");
            if (logger == null) throw new ArgumentNullException("logger");
            if (htmlEncoder == null) throw new ArgumentNullException("htmlEncoder");
            if (persistanceStore == null) throw new ArgumentNullException("persistanceStore");
            if (clientScripts == null) throw new ArgumentNullException("clientScripts");
            if (resources == null) throw new ArgumentNullException("pipelineInspectors");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (tabs == null) throw new ArgumentNullException("tabs");
            if (runtimePolicies == null) throw new ArgumentNullException("runtimePolicies");
            if (defaultResource == null) throw new ArgumentNullException("defaultResource");
            if (proxyFactory == null) throw new ArgumentNullException("proxyFactory");
            if (messageBroker == null) throw new ArgumentNullException("messageBroker");
            if (endpointBaseUri == null) throw new ArgumentNullException("endpointBaseUri");

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
            EndpointBaseUri = endpointBaseUri;
        }

        private ICollection<IClientScript> clientScripts;
        public ICollection<IClientScript> ClientScripts
        {
            get
            {
                return clientScripts;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                clientScripts = value;
            }
        }

        private IFrameworkProvider frameworkProvider;
        public IFrameworkProvider FrameworkProvider
        {
            get
            {
                return frameworkProvider;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                frameworkProvider = value;
            }
        }

        private IHtmlEncoder htmlEncoder;
        public IHtmlEncoder HtmlEncoder
        {
            get
            {
                return htmlEncoder;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                htmlEncoder = value;
            }
        }

        private ILogger logger;
        public ILogger Logger
        {
            get
            {
                return logger;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                logger = value;
            }
        }

        private IPersistanceStore persistanceStore;
        public IPersistanceStore PersistanceStore
        {
            get
            {
                return persistanceStore;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                persistanceStore = value;
            }
        }

        private ICollection<IPipelineInspector> pipelineInspectors;
        public ICollection<IPipelineInspector> PipelineInspectors
        {
            get
            {
                return pipelineInspectors;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                pipelineInspectors = value;
            }
        }

        private ResourceEndpointConfiguration resourceEndpoint;
        public ResourceEndpointConfiguration ResourceEndpoint
        {
            get
            {
                return resourceEndpoint;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                resourceEndpoint = value;
            }
        }

        private ICollection<IResource> resources;
        public ICollection<IResource> Resources
        {
            get
            {
                return resources;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                resources = value;
            }
        }

        private ISerializer serializer;
        public ISerializer Serializer
        {
            get
            {
                return serializer;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                serializer = value;
            }
        }

        private ICollection<ITab> tabs;
        public ICollection<ITab> Tabs
        {
            get
            {
                return tabs;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                tabs = value;
            }
        }

        private ICollection<IRuntimePolicy> runtimePolicies;
        public ICollection<IRuntimePolicy> RuntimePolicies
        {
            get
            {
                return runtimePolicies;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                runtimePolicies = value;
            }
        }

        private IResource defaultResource;
        public IResource DefaultResource
        {
            get
            {
                return defaultResource;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                defaultResource = value;
            }
        }

        private IProxyFactory proxyFactory;
        public IProxyFactory ProxyFactory
        {
            get
            {
                return proxyFactory;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                proxyFactory = value;
            }
        }

        private IMessageBroker messageBroker;
        public IMessageBroker MessageBroker
        {
            get
            {
                return messageBroker;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                messageBroker = value;
            }
        }

        public RuntimePolicy DefaultRuntimePolicy { get; set; }

        private string endpointBaseUri;
        public string EndpointBaseUri
        {
            get { return endpointBaseUri; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                endpointBaseUri = value;
            }
        }
    }
}
