using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public static class ResourceParameter
    {
        public static readonly ResourceParameterMetadata RequestId = new ResourceParameterMetadata("RequestId");
        public static readonly ResourceParameterMetadata VersionNumber = new ResourceParameterMetadata("Version", GlimpseRuntime.Version);
        public static readonly ResourceParameterMetadata Callback = new ResourceParameterMetadata("Callback", isRequired:false);
    }
}