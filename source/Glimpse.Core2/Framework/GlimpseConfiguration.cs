using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Glimpse.Core2.Framework
{
    public class GlimpseConfiguration
    {

        //TODO: Add Sanitizer?
        //TODO: Add static FromWebConfig method to construct an instance of GlimpseConfiguration
        public GlimpseConfiguration(IFrameworkProvider frameworkProvider, IGlimpseResourceEndpointConfiguration endpointConfiguration)
        {
            //TODO: Test building glimpse on clean VS install with contracts
            //TODO: Test building glimpse on teamcity with contracts
            Contract.Requires<ArgumentNullException>(frameworkProvider != null, "frameworkProvider");
            Contract.Requires<ArgumentNullException>(endpointConfiguration != null, "endpointConfiguration");

            //TODO: Refactor all these "new" calls to leverage a IOC container?
            FrameworkProvider = frameworkProvider;
            HtmlEncoder = new AntiXssEncoder();
            Logger = new NLogLogger(CreateLogger());
            PersistanceStore = new ApplicationPersistanceStore(frameworkProvider.HttpServerStore);
            PipelineInspectors = new GlimpseCollection<IGlimpsePipelineInspector>();
            ResourceEndpoint = endpointConfiguration;
            Resources = new GlimpseCollection<IGlimpseResource>();
            Serializer = new JsonNetSerializer();
            Tabs = new GlimpseLazyCollection<ITab, IGlimpseTabMetadata>();
            RuntimePolicies = new GlimpseLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata>();
            BasePolicy = RuntimePolicy.Off;
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

        private IGlimpseHtmlEncoder htmlEncoder;
        public IGlimpseHtmlEncoder HtmlEncoder
        {
            get
            {
                Contract.Ensures(Contract.Result<IGlimpseHtmlEncoder>()!=null);
                return htmlEncoder;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                htmlEncoder = value;
            }
        }

        private IGlimpseLogger logger;
        public IGlimpseLogger Logger
        {
            get
            {
                Contract.Ensures(Contract.Result<IGlimpseLogger>()!=null);
                return logger;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                logger = value;
            }
        }

        private IGlimpsePersistanceStore persistanceStore;
        public IGlimpsePersistanceStore PersistanceStore
        {
            get
            {
                Contract.Ensures(Contract.Result<IGlimpsePersistanceStore>()!=null);
                return persistanceStore;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                persistanceStore = value;
            }
        }

        private GlimpseCollection<IGlimpsePipelineInspector> pipelineInspectors;
        public GlimpseCollection<IGlimpsePipelineInspector> PipelineInspectors
        {
            get
            {
                Contract.Ensures(Contract.Result<GlimpseCollection<IGlimpsePipelineInspector>>()!=null);
                return pipelineInspectors;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                pipelineInspectors = value;
            }
        }

        private IGlimpseResourceEndpointConfiguration resourceEndpoint;
        public IGlimpseResourceEndpointConfiguration ResourceEndpoint
        {
            get
            {
                Contract.Ensures(Contract.Result<IGlimpseResourceEndpointConfiguration>()!=null);
                return resourceEndpoint;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                resourceEndpoint = value;
            }
        }

        private GlimpseCollection<IGlimpseResource> resources;
        public GlimpseCollection<IGlimpseResource> Resources
        {
            get
            {
                Contract.Ensures(Contract.Result<GlimpseCollection<IGlimpseResource>>()!=null);
                return resources;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null, "value");
                resources = value;
            }
        }

        private IGlimpseSerializer serializer;
        public IGlimpseSerializer Serializer
        {
            get
            {
                Contract.Ensures(Contract.Result<IGlimpseSerializer>()!=null);
                return serializer;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                serializer = value;
            }
        }

        private GlimpseLazyCollection<ITab, IGlimpseTabMetadata> tabs;
        public GlimpseLazyCollection<ITab, IGlimpseTabMetadata> Tabs
        {
            get
            {
                Contract.Ensures(Contract.Result<GlimpseLazyCollection<ITab, IGlimpseTabMetadata>>()!=null);
                return tabs;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                tabs = value;
            }
        }

        private GlimpseLazyCollection<IRuntimePolicy,IRuntimePolicyMetadata> runtimePolicies;
        public GlimpseLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata> RuntimePolicies
        {
            get
            {
                Contract.Ensures(Contract.Result<GlimpseLazyCollection<IRuntimePolicy, IRuntimePolicyMetadata>>() != null);
                return runtimePolicies;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value!=null,"value");
                runtimePolicies = value;
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
