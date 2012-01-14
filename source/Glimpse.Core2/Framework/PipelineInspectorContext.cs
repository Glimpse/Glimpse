using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class PipelineInspectorContext : IPipelineInspectorContext
    {
        public ILogger Logger { get; set; }

        public PipelineInspectorContext(ILogger logger)
        {
            Logger = logger;
        }
    }
}