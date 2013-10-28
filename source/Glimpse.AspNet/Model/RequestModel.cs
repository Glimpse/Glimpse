using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Reflection;

namespace Glimpse.AspNet.Model
{
    public class RequestModel
    {
        private static readonly FieldInfo cookieField = typeof(HttpRequest).GetField("_cookies",
                                                              BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo formField = typeof(HttpRequest).GetField("_form",
                                                              BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo httpHeadersField = typeof(HttpRequest).GetField("_headers",
                                                              BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo httpRequestField = typeof(HttpRequestWrapper).GetField("_httpRequest",
                                                                      BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo queryStringField = typeof(HttpRequest).GetField("_queryString",
                                                                      BindingFlags.Instance | BindingFlags.NonPublic);

        public RequestModel(HttpContextBase context)
        {
            var request = context.Request;
            var httpRequest = httpRequestField.GetValue(request) as HttpRequest;

            CurrentUiCulture = Thread.CurrentThread.CurrentUICulture;
            ApplicationPath = request.ApplicationPath;
            AppRelativeCurrentExecutionFilePath = request.AppRelativeCurrentExecutionFilePath;
            CurrentExecutionFilePath = request.CurrentExecutionFilePath;
            FilePath = request.FilePath;
            FormVariables = GetFormVariables(httpRequest);
            HeaderFields = GetHeaderFields(httpRequest);
            Path = request.Path;
            PathInfo = request.PathInfo;
            PhysicalApplicationPath = request.PhysicalApplicationPath;
            PhysicalPath = request.PhysicalPath;
            RawUrl = request.RawUrl;
            Url = request.Url;
            UrlReferrer = request.UrlReferrer;
            UserAgent = request.UserAgent;
            UserHostAddress = request.UserHostAddress;
            UserHostName = request.UserHostName;

            Cookies = GetCookies(httpRequest, context.Server);
            QueryString = GetQueryString(httpRequest);
        }

        //// TODO: Add InputStream

        public CultureInfo CurrentUiCulture { get; private set; }

        public string ApplicationPath { get; private set; }

        public string AppRelativeCurrentExecutionFilePath { get; private set; }

        public string CurrentExecutionFilePath { get; private set; }

        public string FilePath { get; private set; }

        public IEnumerable<FormVariable> FormVariables { get; private set; }

        public IEnumerable<HeaderField> HeaderFields { get; private set; }

        public string Path { get; private set; }

        public string PathInfo { get; private set; }

        public string PhysicalApplicationPath { get; private set; }

        public string PhysicalPath { get; private set; }

        public string RawUrl { get; private set; }

        public Uri Url { get; private set; }

        public Uri UrlReferrer { get; private set; }

        public string UserAgent { get; private set; }

        public string UserHostAddress { get; private set; }

        public string UserHostName { get; private set; }

        public IEnumerable<Cookie> Cookies { get; private set; }

        public IEnumerable<QueryStringParameter> QueryString { get; private set; }


        private IEnumerable<Cookie> GetCookies(HttpRequest httpRequest, HttpServerUtilityBase server)
        {
            if (httpRequest != null)
            {
                var cookies = cookieField.GetValue(httpRequest) as HttpCookieCollection;

                if (cookies != null)
                {
                    foreach (var key in cookies.AllKeys)
                    {
                        var cookie = cookies[key];

                        yield return new Cookie
                        {
                            Name = cookie.Name,
                            Path = cookie.Path,
                            IsSecure = cookie.Secure,
                            Value = server.UrlDecode(cookie.Value)
                        };
                    }
                }
            }
        }
       
        private IEnumerable<FormVariable> GetFormVariables(HttpRequest httpRequest)
        {
            if (httpRequest != null)
            {
                var formVariables = formField.GetValue(httpRequest) as NameValueCollection;

                if (formVariables != null)
                {
                    foreach (var key in formVariables.AllKeys)
                    {
                        yield return new FormVariable { Key = key, Value = formVariables[key] };
                    }
                }
            }
        }

        private IEnumerable<HeaderField> GetHeaderFields(HttpRequest httpRequest)
        {
            if (httpRequest != null)
            {
                var headerFields = httpHeadersField.GetValue(httpRequest) as NameValueCollection;

                if (headerFields != null)
                {
                    foreach (var key in headerFields.AllKeys)
                    {
                        yield return new HeaderField { Key = key, Value = headerFields[key] };
                    }
                }
            }
        }

        private IEnumerable<QueryStringParameter> GetQueryString(HttpRequest httpRequest)
        {
            if (httpRequest != null)
            {
                var queryString = queryStringField.GetValue(httpRequest) as NameValueCollection;

                if (queryString != null)
                {
                    foreach (var key in queryString.AllKeys)
                    {
                        yield return new QueryStringParameter { Key = key, Value = queryString[key] };
                    }
                }
            }
        }

        public class FormVariable
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class HeaderField
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class QueryStringParameter
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        public class Cookie
        {
            public string Name { get; set; }

            public string Path { get; set; }

            public bool IsSecure { get; set; }

            public string Value { get; set; }
        }
    }
}