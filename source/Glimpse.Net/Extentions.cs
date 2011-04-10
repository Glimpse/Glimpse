using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
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

            var validIp = store["__validIp"] as bool?;
            if (!validIp.HasValue)
                store["__validIp"] = validIp = config.IpAddresses.Contains(request.ServerVariables["REMOTE_ADDR"]);

            if (!validIp.Value) return false;

            if (checkMode)
            {
                var validMode = store["__validMode"] as bool?;
                if (!validMode.HasValue)
                    store["__validMode"] = validMode = application.GetGlimpseMode() == GlimpseMode.On;

                if (!validMode.Value) return false;
            }

            if (checkContentType)
            {
                var validContentType = store["__valueContentType"] as bool?;
                if (!validContentType.HasValue)
                    store["__valueContentType"] = validContentType = config.ContentTypes.Contains(response.ContentType);

                if (!validContentType.Value) return false;
            }

            if (checkPath)
            {
                var validPath = store["__validPath"] as bool?;
                if (validPath.HasValue && validPath.Value == true) return false;
            }

            if (response.StatusCode != 200) return false;

            return true;
        }

        public static bool IsAjax(this HttpApplication application)
        {
            return new HttpRequestWrapper(application.Request).IsAjaxRequest();
        }

        public static string GetClientName(this HttpApplication application)
        {
            var cookie = application.Request.Cookies[GlimpseConstants.CookieClientNameKey];
            return cookie != null ? cookie.Value : "";
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

        public static IDictionary<string, string> Flatten(this IDictionary<string, object> dictionary)
        {
            var result = new Dictionary<string, string>();

            if (dictionary == null) return null;

            foreach (var key in dictionary.Keys)
            {
                result.Add(key, dictionary[key].ToString());
            }

            if (result.Count == 0) return null;

            return result;
        }

        public static IDictionary<string, string> Flatten(this NameValueCollection collection)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in collection.AllKeys)
            {
                var keyValue = string.IsNullOrEmpty(key) ? "*--*" : key;
                result.Add(keyValue, collection[keyValue]);
            }

            if (result.Count == 0) return null;

            return result;
        }

        public static string GetName(this ActionDescriptor mvcActionDescriptor)
        {
            var sb = new StringBuilder(mvcActionDescriptor.ActionName + "(");

            var parameters = mvcActionDescriptor.GetParameters();
            foreach (var parameter in parameters)
            {
                sb.Append(parameter.ParameterType + " ");
                sb.Append(parameter.ParameterName + ", ");
            }
            if (parameters.Length > 0) sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }

        public static void Save(this IDictionary dictionary, object key, object value)
        {
            if (dictionary.Contains(key)) return;

            dictionary.Add(key, value);
        }

    }
}
