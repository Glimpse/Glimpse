using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public class RuntimePolicyContext : IRuntimePolicyContext
    {
        public IRequestMetadata RequestMetadata { get; set; }

        public IGlimpseLogger Logger { get; set; }

        public RuntimePolicyContext(IRequestMetadata requestMetadata, IGlimpseLogger logger)
        {
            RequestMetadata = requestMetadata;
            Logger = logger;
        }
    }
}