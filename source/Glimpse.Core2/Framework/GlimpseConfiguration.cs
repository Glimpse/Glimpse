using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class GlimpseConfiguration
    {
        //TODO: Add Sanitizer?
        public GlimpseConfiguration(IFrameworkProvider frameworkProvider, 
            ResourceEndpointConfiguration endpointConfiguration,
            ICollection<IClientScript> clientScripts,
            ILogger logger,
            RuntimePolicy baseRuntimePolicy,
            IHtmlEncoder htmlEncoder,
            IPersistanceStore persistanceStore,
            ICollection<IPipelineInspector> pipelineInspectors)
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

            //TODO: Refactor all these "new" calls to leverage a IOC container?
            Logger = logger;
            ClientScripts = clientScripts;
            FrameworkProvider = frameworkProvider;
            HtmlEncoder = htmlEncoder;
            PersistanceStore = persistanceStore;
            PipelineInspectors = pipelineInspectors;
            ResourceEndpoint = endpointConfiguration;
            Resources = new DiscoverableCollection<IResource>();
            Serializer = new JsonNetSerializer();
            Tabs = new DiscoverableLazyCollection<ITab, ITabMetadata>();
            RuntimePolicies = new DiscoverableLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata>();
            BaseRuntimePolicy = baseRuntimePolicy;
            SerializationConverters = new DiscoverableCollection<ISerializationConverter>();
            DefaultResourceName = Resource.Configuration.InternalName;
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

        private DiscoverableCollection<IResource> resources;
        public DiscoverableCollection<IResource> Resources
        {
            get
            {
                Contract.Ensures(Contract.Result<DiscoverableCollection<IResource>>()!=null);
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

        private DiscoverableLazyCollection<ITab, ITabMetadata> tabs;
        public DiscoverableLazyCollection<ITab, ITabMetadata> Tabs
        {
            get
            {
                Contract.Ensures(Contract.Result<DiscoverableLazyCollection<ITab, ITabMetadata>>()!=null);
                return tabs;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                tabs = value;
            }
        }

        private DiscoverableLazyCollection<IRuntimePolicy,IRuntimePolicyMetadata> runtimePolicies;
        public DiscoverableLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata> RuntimePolicies
        {
            get
            {
                Contract.Ensures(Contract.Result<DiscoverableLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata>>() != null);
                return runtimePolicies;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                runtimePolicies = value;
            }
        }

        private DiscoverableCollection<ISerializationConverter> serializationConverters;
        public DiscoverableCollection<ISerializationConverter> SerializationConverters
        {
            get
            {
                Contract.Ensures(Contract.Result<DiscoverableCollection<ISerializationConverter>>()!=null);
                return serializationConverters;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                serializationConverters = value;
            }
        }

        private string defaultResourceName;
        public string DefaultResourceName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return defaultResourceName;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(value), "value");
                defaultResourceName = value;
            }
        }

        public RuntimePolicy BaseRuntimePolicy { get; set; }
    }
}
