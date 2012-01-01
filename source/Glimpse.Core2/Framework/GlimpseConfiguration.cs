using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

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

            FrameworkProvider = frameworkProvider;
            HtmlEncoder = new AntiXssEncoder();
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

        public IGlimpsePersistanceStore PersistanceStore { get; set; }

        public GlimpseCollection<IGlimpsePipelineInspector> PipelineInspectors { get; set; }

        public IGlimpseResourceEndpointConfiguration ResourceEndpoint { get; set; }

        public GlimpseCollection<IGlimpseResource> Resources { get; set; }

        public IGlimpseSerializer Serializer { get; set; }

        public GlimpseLazyCollection<IGlimpseTab, IGlimpseTabMetadata> Tabs { get; set; }

        public GlimpseValidatorCollection Validators { get; set; }
    }
}
