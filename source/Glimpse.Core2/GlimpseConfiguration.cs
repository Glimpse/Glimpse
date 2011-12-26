using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class GlimpseConfiguration
    {
        public GlimpseLazyCollection<IGlimpseTab, IGlimpsePluginMetadata> Plugins { get; set; }
        public GlimpseCollection<IGlimpsePipelineModifier> PipelineModifiers { get; set; }
        public GlimpseCollection<IGlimpseResource> Resources { get; set; }
        public GlimpseValidatorCollection Validators { get; set; }
        public IFrameworkProvider FrameworkProvider { get; set; }
        public IGlimpseSerializer Serializer { get; set; }
        public IGlimpseHtmlEncoder HtmlEncoder { get; set; }
        public IGlimpsePersistanceStore PersistanceStore { get; set; }
        public IGlimpseResourceEndpointConfiguration ResourceEndpoint { get; set; }

        public GlimpseConfiguration(IFrameworkProvider frameworkProvider, IGlimpseResourceEndpointConfiguration endpointConfiguration)
        {
            Contract.Requires(frameworkProvider != null);
            //only use contracts if we can build them on teamcity AND the experience is nice for users that don't have the VS extension installed

            FrameworkProvider = frameworkProvider;
            Plugins = new GlimpseLazyCollection<IGlimpseTab, IGlimpsePluginMetadata>();
            PipelineModifiers = new GlimpseCollection<IGlimpsePipelineModifier>();
            Resources = new GlimpseCollection<IGlimpseResource>();
            Serializer = new JsonNetSerializer();
            PersistanceStore = new ApplicationPersistanceStore(frameworkProvider.HttpServerStore);
            HtmlEncoder = new AntiXssEncoder();
            ResourceEndpoint = endpointConfiguration;
            Validators = new GlimpseValidatorCollection();
        }
    }
}
