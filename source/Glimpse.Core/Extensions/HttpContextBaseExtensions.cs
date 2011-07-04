using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Extensions
{
    public static class HttpContextBaseExtensions
    {
        internal static GlimpseRequestMetadata GetRequestMetadata(this HttpContextBase context, string jsonPayload)
        {
            var browser = context.Request.Browser;
            return new GlimpseRequestMetadata
                       {
                           Browser = string.Format("{0} {1}", browser.Browser, browser.Version),
                           ClientName = context.GetClientName(),
                           Json = jsonPayload,
                           RequestTime = DateTime.Now.ToLongTimeString(),
                           RequestId = context.GetGlimpseRequestId(),
                           IsAjax = context.IsAjax().ToString(),
                           Url = context.Request.RawUrl,
                           Method = context.Request.HttpMethod
                       };
        }

        internal static bool IsAjax(this HttpContextBase context)
        {
            var request = context.Request;

            return ((request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }

        internal static string GetClientName(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies[GlimpseConstants.CookieClientNameKey];
            return cookie != null ? cookie.Value : "";
        }

        internal static GlimpseMode GetGlimpseMode(this HttpContextBase context)
        {
            var cookies = context.Request.Cookies;

            var cookie = cookies[GlimpseConstants.CookieModeKey];
            if (cookie == null) return GlimpseMode.Off;

            var mode = GlimpseMode.Off;
            if (!GlimpseMode.TryParse(cookie.Value, true, out mode)) return GlimpseMode.Off;

            return mode;
        }

        internal static void InitGlimpseContext(this HttpContextBase context)
        {
            context.Items[GlimpseConstants.Context] = new Dictionary<string, object>();
        }

        internal static bool TryGetData(this HttpContextBase context, out IDictionary<string, object> data)
        {
            data = context.Items[GlimpseConstants.Context] as IDictionary<string, object>;

            return (data != null);
        }

        public static string GlimpseResourcePath(this HttpContextBase context, string resource)
        {
            var root = VirtualPathUtility.ToAbsolute("~/", context.Request.ApplicationPath);

            if (resource == null) return string.Format("{0}Glimpse.axd", root);

            return string.Format("{0}Glimpse.axd?{3}={4}&{2}={1}", root, resource, Handler.ResourceKey, Handler.VersionKey, Module.RunningVersion);
        }

/*        public static List<IGlimpseWarning> GetWarnings(this HttpContextBase context)
        {
            var result = context.Items[GlimpseConstants.Warnings] as List<IGlimpseWarning>;
            if (result == null) context.Items[GlimpseConstants.Warnings] = result = new List<IGlimpseWarning>();

            return result;
        }*/

        public static Guid GetGlimpseRequestId(this HttpContextBase context)
        {
            Guid result;
            try
            {
                result = (Guid) context.Items[GlimpseConstants.GlimpseRequestId];
            }
            catch
            {
                context.Items[GlimpseConstants.GlimpseRequestId] = result = Guid.NewGuid();
            }

            return result;
        }
    }
}
