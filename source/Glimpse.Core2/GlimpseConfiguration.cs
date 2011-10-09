using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class GlimpseConfiguration
    {
        public GlimpseLazyCollection<IGlimpsePlugin, IGlimpsePluginMetadata> Plugins { get; set; }
        public GlimpseCollection<IGlimpsePipelineModifier> PipelineModifiers { get; set; }
        public IFrameworkProvider FrameworkProvider { get; set; }

        public GlimpseConfiguration(IFrameworkProvider frameworkProvider)
        {
            Contract.Requires(frameworkProvider != null);

            FrameworkProvider = frameworkProvider;
            Plugins = new GlimpseLazyCollection<IGlimpsePlugin, IGlimpsePluginMetadata>();
            PipelineModifiers = new GlimpseCollection<IGlimpsePipelineModifier>();
        }
    }
}
