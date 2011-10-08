using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class GlimpseConfiguration
    {
        public PluginCollection Plugins { get; set; }
        public IRuntimeService RuntimeService { get; set; }

        public GlimpseConfiguration(IRuntimeService runtimeService)
        {
            RuntimeService = runtimeService;
        }
    }
}
