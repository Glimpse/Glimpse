using System;
using System.Linq;
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

        private static readonly FieldInfo filesField = typeof(HttpRequest).GetField("_files",
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
            
            //client side
            HttpBrowserCapabilitiesBase browser = request.Browser;
            IEnumerable<Cookie> cookies = GetCookies(httpRequest, context.Server);
            IEnumerable<HttpPostedFile> files = GetPostedFiles(httpRequest);
            IEnumerable<FormVariable> formVariables = GetFormVariables(httpRequest);
            IEnumerable<HeaderField> headerFields = GetHeaderFields(httpRequest);
            IEnumerable<QueryStringParameter> queryString = GetQueryString(httpRequest);
            string rawUrl = request.RawUrl;
            string requestType = request.RequestType;
            Uri url = request.Url;
            Uri urlReferrer = request.UrlReferrer;
            string userAgent = request.UserAgent;
            string userHostAddress = request.UserHostAddress;
            string userHostName = request.UserHostName;
            
            ClientSide = new ClientSide
            {
                Browser = browser,
                Cookies = cookies,
                Files = files,
                HeaderFields = headerFields,
                FormVariables = formVariables,
                QueryString = queryString,
                RawUrl = rawUrl,
                RequestType = requestType,
                Url = url,
                UrlReferrer = urlReferrer,
                UserAgent = userAgent,
                UserHostAddress = userHostAddress,
                UserHostName = userHostName
            };

            //server side
            string applicationPath = request.ApplicationPath;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            string appRelativeCurrentExecutionFilePath = request.AppRelativeCurrentExecutionFilePath;
            string currentExecutionFilePath = request.CurrentExecutionFilePath;
            string filePath = request.FilePath;
            string path = request.Path;
            string pathInfo = request.PathInfo;
            string physicalApplicationPath = request.PhysicalApplicationPath;
            string physicalPath = request.PhysicalPath;

            ServerSide = new ServerSide
                {
                    ApplicationPath = applicationPath, 
                    AppRelativeCurrentExecutionFilePath = appRelativeCurrentExecutionFilePath, 
                    CurrentUiCulture = currentUiCulture,
                    CurrentExecutionFilePath = currentExecutionFilePath,
                    FilePath = filePath,
                    Path = path,
                    PathInfo = pathInfo,
                    PhysicalApplicationPath = physicalApplicationPath,
                    PhysicalPath = physicalPath
                };
        }

        //// TODO: Add InputStream

        public ServerSide ServerSide { get; private set; }

        public ClientSide ClientSide { get; private set; }

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

        private IEnumerable<HttpPostedFile> GetPostedFiles(HttpRequest httpRequest)
        {
            if (httpRequest != null)
            {
                var files = filesField.GetValue(httpRequest) as HttpFileCollection;

                if (files != null)
                {
                    foreach (var key in files.AllKeys)
                    {
                        yield return files[key];
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
                    foreach (var key in headerFields.AllKeys.Where(k => k.ToLower() != "cookie"))
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
            public string Value { get; set; }
            public string Path { get; set; }
            public bool IsSecure { get; set; }
        }
    }


    public class ClientSide
    {
        public HttpBrowserCapabilitiesBase Browser { get; set; }
        public IEnumerable<RequestModel.Cookie> Cookies { get; set; }
        public IEnumerable<HttpPostedFile> Files { get; set; }
        public IEnumerable<RequestModel.FormVariable> FormVariables { get; set; }
        public IEnumerable<RequestModel.HeaderField> HeaderFields { get; set; }
        public IEnumerable<RequestModel.QueryStringParameter> QueryString { get; set; }
        public string RawUrl { get; set; }
        public string RequestType { get; set; }
        public Uri Url { get; set; }
        public Uri UrlReferrer { get; set; }
        public string UserAgent { get; set; }
        public string UserHostAddress { get; set; }
        public string UserHostName { get; set; }
    }

    public class ServerSide
    {
        public string ApplicationPath { get; set; }
        public string AppRelativeCurrentExecutionFilePath { get; set; }
        public CultureInfo CurrentUiCulture { get; set; }
        public string CurrentExecutionFilePath { get; set; }
        public string FilePath { get; set; }
        public string Path { get; set; }
        public string PathInfo { get; set; }
        public string PhysicalApplicationPath { get; set; }
        public string PhysicalPath { get; set; }
    }

}