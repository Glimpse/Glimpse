using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Extensions
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
                    store[GlimpseConstants.ValidMode] = validMode = application.GetGlimpseMode() == GlimpseMode.On;

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
            return new HttpRequestWrapper(application.Request).IsAjaxRequest();
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

        internal static IList<object[]> GetWarningStore(this HttpApplication application)
        {
            var result = application.Context.Items[GlimpseConstants.Errors] as IList<object[]>;
            if (result == null) application.Context.Items[GlimpseConstants.Errors] = result = new List<object[]>{new []{"Type", "Message"}};

            return result;
        }
    }
}
