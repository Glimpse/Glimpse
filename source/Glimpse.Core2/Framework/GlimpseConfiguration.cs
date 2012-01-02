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
        //TODO: Add Logger
        //TODO: Add static FromWebConfig method to construct an instance of GlimpseConfiguration
        public GlimpseConfiguration(IFrameworkProvider frameworkProvider, IGlimpseResourceEndpointConfiguration endpointConfiguration)
        {
            //TODO: Test building glimpse on clean VS install with contracts
            //TODO: Test building glimpse on teamcity with contracts
            Contract.Requires<ArgumentNullException>(frameworkProvider != null, "frameworkProvider");
            Contract.Requires<ArgumentNullException>(endpointConfiguration != null, "endpointConfiguration");

            Contract.Ensures(FrameworkProvider != null);
            Contract.Ensures(HtmlEncoder != null);
            Contract.Ensures(PersistanceStore != null);
            Contract.Ensures(PipelineInspectors != null);
            Contract.Ensures(ResourceEndpoint != null);
            Contract.Ensures(Resources != null);
            Contract.Ensures(Serializer != null);
            Contract.Ensures(Tabs != null);
            Contract.Ensures(Validators != null);

            //TODO: Refactor all these "new" calls to leverage a IOC container
            FrameworkProvider = frameworkProvider;
            HtmlEncoder = new AntiXssEncoder();
            Logger = new NLogLogger(CreateLogger());
            PersistanceStore = new ApplicationPersistanceStore(frameworkProvider.HttpServerStore);
            PipelineInspectors = new GlimpseCollection<IGlimpsePipelineInspector>();
            ResourceEndpoint = endpointConfiguration;
            Resources = new GlimpseCollection<IGlimpseResource>();
            Serializer = new JsonNetSerializer();
            Tabs = new GlimpseLazyCollection<IGlimpseTab, IGlimpseTabMetadata>();
            Validators = new GlimpseValidatorCollection();

        }

        public IFrameworkProvider FrameworkProvider { get; set; }

        public IGlimpseHtmlEncoder HtmlEncoder { get; set; }

        public IGlimpseLogger Logger { get; set; }

        public IGlimpsePersistanceStore PersistanceStore { get; set; }

        public GlimpseCollection<IGlimpsePipelineInspector> PipelineInspectors { get; set; }

        public IGlimpseResourceEndpointConfiguration ResourceEndpoint { get; set; }

        public GlimpseCollection<IGlimpseResource> Resources { get; set; }

        public IGlimpseSerializer Serializer { get; set; }

        public GlimpseLazyCollection<IGlimpseTab, IGlimpseTabMetadata> Tabs { get; set; }

        public GlimpseValidatorCollection Validators { get; set; }

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
