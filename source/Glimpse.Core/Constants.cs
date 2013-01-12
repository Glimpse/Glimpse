namespace Glimpse.Core
{
    /// <summary>
    /// Class Constants used to hold constants that are used through the system
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The client id cookie name
        /// </summary>
        public const string ClientIdCookieName = "glimpseId";
        /// <summary>
        /// The HTTP response header
        /// </summary>
        public const string HttpResponseHeader = "X-Glimpse-RequestID";
        /// <summary>
        /// The user agent header name
        /// </summary>
        public const string UserAgentHeaderName = "User-Agent";
        /// <summary>
        /// The HTTP request header
        /// </summary>
        public const string HttpRequestHeader = "X-Glimpse-Parent-RequestID";

        /// <summary>
        /// The plugin results data store key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string PluginResultsDataStoreKey = "__GlimpsePluginResultsKey";
        /// <summary>
        /// The request id key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string RequestIdKey = "__GlimpseRequestId";
        /// <summary>
        /// The global stopwatch key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string GlobalStopwatchKey = "__GlimpseGlobalStopwatch";
        /// <summary>
        /// The runtime policy key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string RuntimePolicyKey = "__GlimpseRequestRuntimePermissions";
        /// <summary>
        /// The global timer key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string GlobalTimerKey = "__GlimpseTimer";
        /// <summary>
        /// The tab storage key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string TabStorageKey = "__GlimpseTabStorage";
    }
}