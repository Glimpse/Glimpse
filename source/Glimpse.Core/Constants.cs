namespace Glimpse.Core
{
    /// <summary>
    /// Common constant strings used throughout Glimpse.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The name of the cookie Glimpse will use to keep track of a user's session.
        /// </summary>
        public const string ClientIdCookieName = "glimpseId";
        
        /// <summary>
        /// The name of the Http response header the Glimpse server will write the request ID to. 
        /// </summary>
        /// <remarks>
        /// In the past <see cref="HttpResponseHeader"/> has used an "X-" prefix to denote a custom header, but that practice has been deprecated as of <seealso href="http://tools.ietf.org/html/rfc6648">RFC 6648</seealso>.
        /// </remarks>
        public const string HttpResponseHeader = "Glimpse-RequestID";
        
        /// <summary>
        /// The name of the Http request header which contains the client's user agent string as defined in <seealso href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.43">Section 14.43 of RFC 2616 - Hypertext Transfer Protocol (HTTP/1.1)</seealso>.
        /// </summary>
        public const string UserAgentHeaderName = "User-Agent";

        /// <summary>
        /// The name of the Http request header the Glimpse client will write the parent request ID to. 
        /// </summary>
        /// <remarks>
        /// In the past <see cref="HttpRequestHeader"/> has used an "X-" prefix to denote a custom header, but that practice has been deprecated as of <seealso href="http://tools.ietf.org/html/rfc6648">RFC 6648</seealso>.
        /// </remarks>
        public const string HttpRequestHeader = "Glimpse-Parent-RequestID";

        /// <summary>
        /// The key Glimpse server uses to store an <c>IDictionary&lt;string, IDataStore&gt;</c> which provides each <see cref="Glimpse.Core.Extensibility.ITab"/> implementation a thread safe storage mechanism.
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        public const string TabStorageKey = "__GlimpseTabStorage";

        /// <summary>
        /// The plugin results data store key
        /// </summary>
        /// <remarks>
        /// Typically used as the key for the local request storage (i.e. HttpContext.Items)
        /// </remarks>
        internal const string TabResultsDataStoreKey = "__GlimpseTabResults";

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
    }
}