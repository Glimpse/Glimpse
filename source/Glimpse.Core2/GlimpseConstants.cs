namespace Glimpse.Core2
{
    public static class Constants
    {
        public const string ControlCookieName = "glimpsePolicy";
        public const string HttpHeader = "X-Glimpse-RequestID";

        internal const string PluginResultsDataStoreKey = "__GlimpsePluginResultsKey";
        internal const string PluginsDataStoreKey = "__GlimpsePluginsKey";
        internal const string ServiceLocatorKey = "__GlimpseServiceLocator";
        internal const string RequestIdKey = "__GlimpseRequestId";
        internal const string GlobalStopwatchKey = "__GlimpseGlobalStopwatch";
        internal const string RuntimePermissionsKey = "__GlimpseRequestRuntimePermissions";
    }
}