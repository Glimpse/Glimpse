using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Configuration;

namespace Glimpse.Net
{
    public static class Extentions
    {
        public static bool IsValidRequest(this HttpApplication application, GlimpseConfiguration config, bool checkContentType, bool checkMode = true, bool checkPath = true)
        {
            return IsValidRequestInternal(application, config, checkContentType, checkMode, checkPath);
        }

        public static bool IsValidRequest(this object obj, out HttpApplication application, GlimpseConfiguration config, bool checkContentType, bool checkMode = true, bool checkPath = true)
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
            var cookies = request.Cookies;

            //check IP address
            var validIp = store["__validIp"] as bool?;
            if (!validIp.HasValue)
                store["__validIp"] = validIp = config.IpAddresses.Contains(request.ServerVariables["REMOTE_ADDR"]);

            if (!validIp.Value) return false;
            //end check

            if (checkMode)
            {
                //check cookie
                var validMode = store["__validMode"] as bool?;
                if (!validMode.HasValue)
                    store["__validMode"] = validMode = application.GetGlimpseMode() == GlimpseMode.On;

                if (!validMode.Value) return false;
                //end check
            }

            if (checkContentType)
            {
                //check contentType
                var validContentType = store["__valueContentType"] as bool?;
                if (!validContentType.HasValue)
                    store["__valueContentType"] = validContentType = config.ContentTypes.Contains(response.ContentType);

                if (!validContentType.Value) return false;
                //end check
            }

            if (checkPath)
            {
                //check path
                var validPath = store["__validPath"] as bool?;
                if (validPath.HasValue && validPath.Value == true) return false;
                //end check)
            }

            return true;
        }

        public static bool IsAjax(this HttpApplication application)
        {
            return new HttpRequestWrapper(application.Request).IsAjaxRequest();
        }

        public static bool IsGlimpseRequest(this HttpApplication application)
        {
            var path = application.Request.Path;

            var result = (path.StartsWith("/Glimpse/Config") ||
                    path.StartsWith("/Glimpse/History") ||
                    path.StartsWith("/Glimpse/Clients"));

            application.Context.Items["__validPath"] = result;
            return result;
        }

        public static GlimpseMode GetGlimpseMode(this HttpApplication application)
        {
            var cookies = application.Request.Cookies;

            var cookie = cookies[GlimpseConstants.CookieModeKey];
            if (cookie == null) return GlimpseMode.Off;

            var mode = GlimpseMode.Off;
            if (!GlimpseMode.TryParse(cookie.Value, true, out mode)) return GlimpseMode.Off;

            return mode;
        }

        public static void SetData(this HttpApplication application, IDictionary<string, object> data)
        {
            application.Context.Items[GlimpseConstants.Data] = data;
        }

        public static bool TryGetData(this HttpApplication application, out IDictionary<string, object> data)
        {
            data = application.Context.Items[GlimpseConstants.Data] as IDictionary<string, object>;

            return (data != null);
        }
    }
}
