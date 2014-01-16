using Glimpse.Core.Extensibility;

namespace Glimpse.Core
{
    /// <summary>
    /// Common constant strings used throughout Glimpse.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The key Glimpse server uses to store an <c>IDictionary&lt;string, IDataStore&gt;</c> which provides each <see cref="Glimpse.Core.Extensibility.ITab"/> implementation a thread safe storage mechanism.
        /// </summary>
        /// <remarks>
        /// Used as the key for the framework provider's local request storage mechanism (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        internal const string TabStorageKey = "__GlimpseTabStorage";

        /// <summary>
        /// The name of the cookie Glimpse will use to keep track of a user's session.
        /// </summary>
        internal const string ClientIdCookieName = "glimpseId";

        /// <summary>
        /// The key Glimpse server uses to store a <see cref="System.Diagnostics.Stopwatch"/> for tracking execution duration.
        /// </summary>
        /// <remarks>
        /// Used as the key for the framework provider's local request storage mechanism (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        internal const string GlobalStopwatchKey = "__GlimpseGlobalStopwatch";

        /// <summary>
        /// The key Glimpse server uses to store a <see cref="Glimpse.Core.Extensibility.IExecutionTimer"/> for tracking execution duration.
        /// </summary>
        /// <remarks>
        /// Used as the key for the framework provider's local request storage mechanism (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        internal const string GlobalTimerKey = "__GlimpseTimer";

        /// <summary>
        /// The name of the Http request header the Glimpse client will write the parent request ID to. 
        /// </summary>
        /// <remarks>
        /// In the past <see cref="HttpRequestHeader"/> has used an "X-" prefix to denote a custom header, but that practice has been deprecated as of <see href="http://tools.ietf.org/html/rfc6648">RFC 6648</see>.
        /// </remarks>
        internal const string HttpRequestHeader = "Glimpse-Parent-RequestID";

        /// <summary>
        /// The name of the Http response header the Glimpse server will write the request ID to. 
        /// </summary>
        /// <remarks>
        /// In the past <see cref="HttpResponseHeader"/> has used an "X-" prefix to denote a custom header, but that practice has been deprecated as of <see href="http://tools.ietf.org/html/rfc6648">RFC 6648</see>.
        /// </remarks>
        internal const string HttpResponseHeader = "Glimpse-RequestID";

        /// <summary>
        /// The key Glimpse server uses to store a <see cref="System.Guid"/> which represents the current request's unique identifier.
        /// </summary>
        /// <remarks>
        /// Used as the key for the framework provider's local request storage mechanism (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        internal const string RequestIdKey = "__GlimpseRequestId";

        /// <summary>
        /// The key Glimpse server uses to store a <see cref="RuntimePolicy"/> for tracking Glimpse permissions.
        /// </summary>
        /// <remarks>
        /// Used as the key for the framework provider's local request storage mechanism (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        internal const string RuntimePolicyKey = "__GlimpseRequestRuntimePermissions";

        /// <summary>
        /// The key Glimpse server uses to store an <c>IDictionary&lt;string, TabResult&gt;</c> which stores the result of calling <c>GetData()</c> on each <see cref="Glimpse.Core.Extensibility.ITab"/> implementation.
        /// </summary>
        /// <remarks>
        /// Used as the key for the framework provider's local request storage mechanism (i.e. <c>HttpContext.Items</c>).
        /// </remarks>
        internal const string TabResultsDataStoreKey = "__GlimpseTabResults";

        internal const string DisplayResultsDataStoreKey = "__GlimpseDisplayResults";

        /// <summary>
        /// The name of the Http request header which contains the client's user agent string as defined in <see href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.43">Section 14.43 of RFC 2616 - Hypertext Transfer Protocol (HTTP/1.1)</see>.
        /// </summary>
        internal const string UserAgentHeaderName = "User-Agent";

        /// <summary>
        /// The key Glimpse server uses to track if script tags have been injected into an Http response.
        /// </summary>
        internal const string ScriptsHaveRenderedKey = "__GlimpseScriptHasRendered";

        /// <summary>
        /// The key Glimpse server uses to store the client scripts strategy.
        /// </summary>
        internal const string ClientScriptsStrategy = "__GlimpseClientScriptsStrategy";

        /// <summary>
        /// The key Glimpse server uses to store the Glimpse request context handle.
        /// </summary>
        internal const string GlimpseRequestContextHandle = "__GlimpseRequestContextHandle";
    }
}