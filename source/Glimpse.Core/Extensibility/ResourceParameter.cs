namespace Glimpse.Core.Extensibility
{
    public static class ResourceParameter
    {
        public static readonly ResourceParameterMetadata RequestId = new ResourceParameterMetadata("requestId");
        public static readonly ResourceParameterMetadata VersionNumber = new ResourceParameterMetadata("version");
        public static readonly ResourceParameterMetadata Callback = new ResourceParameterMetadata("callback", isRequired:false);
        public static readonly ResourceParameterMetadata Timestamp = new ResourceParameterMetadata("stamp");
    }
}