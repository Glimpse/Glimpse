namespace Glimpse.Core2
{
    public static class Constants
    {
        public const string ClientIdCookieName = "glimpseId";
        public const string HttpResponseHeader = "X-Glimpse-RequestID";
        public const string UserAgentHeaderName = "User-Agent";
        public const string HttpRequestHeader = "X-Glimpse-Parent-RequestID";

        internal const string PluginResultsDataStoreKey = "__GlimpsePluginResultsKey";
        internal const string RequestIdKey = "__GlimpseRequestId";
        internal const string GlobalStopwatchKey = "__GlimpseGlobalStopwatch";
        internal const string RuntimePermissionsKey = "__GlimpseRequestRuntimePermissions";
        internal const string GlobalTimerKey = "__GlimpseTimer";
        internal const string TabStorageKey = "__GlimpseTabStorage";
    }
}