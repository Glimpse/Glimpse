namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// A collection of commonly used <see cref="ResourceParameterMetadata" /> objects.
    /// </summary>
    public static class ResourceParameter
    {
        /// <summary>
        /// The required 'Request Id' parameter.
        /// </summary>
        public static readonly ResourceParameterMetadata RequestId = new ResourceParameterMetadata("requestId");

        /// <summary>
        /// The required 'Version' parameter used for cache busting.
        /// </summary>
        public static readonly ResourceParameterMetadata VersionNumber = new ResourceParameterMetadata("version");

        /// <summary>
        /// The optional 'Callback' parameter used for JsonP requests.
        /// </summary>
        public static readonly ResourceParameterMetadata Callback = new ResourceParameterMetadata("callback", isRequired: false);

        /// <summary>
        /// The required 'Stamp' parameter.
        /// </summary>
        public static readonly ResourceParameterMetadata Timestamp = new ResourceParameterMetadata("stamp");

        /// <summary>
        /// The optional 'Hash' parameter used for HTTP cache busting.
        /// </summary>
        public static readonly ResourceParameterMetadata Hash = new ResourceParameterMetadata("hash", isRequired: false);

        /// <summary>
        /// The required 'logoname' parameter.
        /// </summary>
        public static readonly ResourceParameterMetadata LogoName = new ResourceParameterMetadata("logoname");
    }
}