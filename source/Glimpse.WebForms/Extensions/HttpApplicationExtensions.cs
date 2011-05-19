using System.Collections.Generic;
using System.Web;
using Glimpse.WebForms.Configuration;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.WebForms.Extensions
{
    public static class HttpApplicationExtensions
    {
        internal static bool IsValidRequest(this HttpApplication application, GlimpseConfiguration config, bool checkContentType, bool checkMode = true, bool checkPath = true)
        {
            return IsValidRequestInternal(application, config, checkContentType, checkMode, checkPath);
        }

        internal static bool IsValidRequest(this object obj, out HttpApplication application, GlimpseConfiguration config, bool checkContentType, bool checkMode = true, bool checkPath = true)
        {
            application = obj as HttpApplication;
            if (application == null) return false;

            return IsValidRequestInternal(application, config, checkContentType, checkMode, checkPath);
        }

        private static bool IsValidRequestInternal(HttpApplication application, GlimpseConfiguration config, bool checkContentType, bool checkMode, bool checkPath = true)
        {
            var request = application.Request;
            var response = application.Response;
            var store = application.Context.Items;

            var validIp = store[GlimpseConstants.ValidIp] as bool?;
            if (!validIp.HasValue)
                store[GlimpseConstants.ValidIp] = validIp = config.IpAddresses.Contains(request.ServerVariables["REMOTE_ADDR"]);

            if (!validIp.Value) return false;

            if (checkMode)
            {
                var validMode = store[GlimpseConstants.ValidMode] as bool?;
                if (!validMode.HasValue)
                    store[GlimpseConstants.ValidMode] = validMode = application.GetGlimpseMode() == GlimpseMode.On || application.GetGlimpseMode() == GlimpseMode.Hidden;

                if (!validMode.Value) return false;
            }

            if (checkContentType)
            {
                var validContentType = store[GlimpseConstants.ValidContentType] as bool?;
                if (!validContentType.HasValue)
                    store[GlimpseConstants.ValidContentType] = validContentType = config.ContentTypes.Contains(response.ContentType);

                if (!validContentType.Value) return false;
            }

            if (checkPath)
            {
                var validPath = store[GlimpseConstants.ValidPath] as bool?;
                if (validPath.HasValue && validPath.Value == true) return false;
            }

            var validResponseCodes = new List<int> {200, 301, 302};
            if (!validResponseCodes.Contains(response.StatusCode)) return false;

            return true;
        }

        internal static bool IsAjax(this HttpApplication application)
        {
            var request = application.Request;

            return ((request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }

        internal static string GetClientName(this HttpApplication application)
        {
            var cookie = application.Request.Cookies[GlimpseConstants.CookieClientNameKey];
            return cookie != null ? cookie.Value : "";
        }

        internal static GlimpseMode GetGlimpseMode(this HttpApplication application)
        {
            var cookies = application.Request.Cookies;

            var cookie = cookies[GlimpseConstants.CookieModeKey];
            if (cookie == null) return GlimpseMode.Off;

            var mode = GlimpseMode.Off;
            if (!GlimpseMode.TryParse(cookie.Value, true, out mode)) return GlimpseMode.Off;

            return mode;
        }

        internal static void InitGlimpseContext(this HttpApplication application)
        {
            application.Context.Items[GlimpseConstants.Context] = new Dictionary<string, object>();
        }

        internal static bool TryGetData(this HttpApplication application, out IDictionary<string, object> data)
        {
            data = application.Context.Items[GlimpseConstants.Context] as IDictionary<string, object>;

            return (data != null);
        }

        public static List<IGlimpseWarning> GetWarnings(this HttpContext context)
        {
            var result = context.Items[GlimpseConstants.Warnings] as List<IGlimpseWarning>;
            if (result == null) context.Items[GlimpseConstants.Warnings] = result = new List<IGlimpseWarning>();

            return result;
        }
    }
}
