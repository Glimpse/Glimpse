using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class HttpContextBaseExtensions
    {
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

        public static List<IGlimpseWarning> GetWarnings(this HttpContextBase context)
        {
            var result = context.Items[GlimpseConstants.Warnings] as List<IGlimpseWarning>;
            if (result == null) context.Items[GlimpseConstants.Warnings] = result = new List<IGlimpseWarning>();

            return result;
        }

        public static Guid GetRequestId(this HttpContextBase context)
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
