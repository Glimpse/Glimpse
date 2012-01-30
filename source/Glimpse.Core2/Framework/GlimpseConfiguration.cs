using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Glimpse.Core2.Framework
{
    public class GlimpseConfiguration
    {

        //TODO: Add Sanitizer?
        //TODO: Add static FromWebConfig method to construct an instance of GlimpseConfiguration
        public GlimpseConfiguration(IFrameworkProvider frameworkProvider, ResourceEndpointConfiguration endpointConfiguration)
        {
            //TODO: Test building glimpse on clean VS install with contracts
            //TODO: Test building glimpse on teamcity with contracts
            Contract.Requires<ArgumentNullException>(frameworkProvider != null, "frameworkProvider");
            Contract.Requires<ArgumentNullException>(endpointConfiguration != null, "endpointConfiguration");

            //TODO: Refactor all these "new" calls to leverage a IOC container?
            ClientScripts = new DiscoverableCollection<IClientScript>();
            FrameworkProvider = frameworkProvider;
            HtmlEncoder = new AntiXssEncoder();
            Logger = new NLogLogger(CreateLogger());
            PersistanceStore = new ApplicationPersistanceStore(frameworkProvider.HttpServerStore);
            PipelineInspectors = new DiscoverableCollection<IPipelineInspector>();
            ResourceEndpoint = endpointConfiguration;
            Resources = new DiscoverableCollection<IResource>();
            Serializer = new JsonNetSerializer();
            Tabs = new DiscoverableLazyCollection<ITab, ITabMetadata>();
            RuntimePolicies = new DiscoverableLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata>();
            BasePolicy = RuntimePolicy.Off;
            SerializationConverters = new DiscoverableCollection<ISerializationConverter>();
            DefaultResourceName = Configuration.InternalName;
        }

        private DiscoverableCollection<IClientScript> clientScripts;
        public DiscoverableCollection<IClientScript> ClientScripts
        {
            get
            {
                Contract.Ensures(Contract.Result<DiscoverableCollection<IClientScript>>()!=null);
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

        private DiscoverableCollection<IPipelineInspector> pipelineInspectors;
        public DiscoverableCollection<IPipelineInspector> PipelineInspectors
        {
            get
            {
                Contract.Ensures(Contract.Result<DiscoverableCollection<IPipelineInspector>>()!=null);
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

        public RuntimePolicy BasePolicy { get; set; }

        //TODO: Remove me! This does not belong here, allow for IOC pipeline config
        private Logger CreateLogger()
        {
            var loggingConfiguration = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration  
            var fileTarget = new FileTarget();
            loggingConfiguration.AddTarget("file", fileTarget);

            // Step 3. Set target properties  
            fileTarget.FileName = "${basedir}/Glimpse.log";
            fileTarget.Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}|${exception:maxInnerExceptionLevel=5:format=type,message,stacktrace:separator=--:innerFormat=shortType,message,method}";

            // Step 4. Define rules 
            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            loggingConfiguration.LoggingRules.Add(rule2);

            return new LogFactory(loggingConfiguration).GetLogger("Glimpse");
        }
    }
}
