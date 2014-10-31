using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Web;

namespace Glimpse.AspNet.Model
{
    public class RequestModel
    {
        public RequestModel(HttpContextBase context)
        {
            var request = context.Request;

            Cookies = GetCookies(request, context.Server);
            CurrentUiCulture = Thread.CurrentThread.CurrentUICulture;
            Files = GetPostedFiles(request);
            FormVariables = GetFormVariables(request);
            HeaderFields = GetHeaderFields(request);
            QueryString = GetQueryString(request);
            RawUrl = request.RawUrl;
            RequestType = request.RequestType;
            Url = request.Url;
            UrlReferrer = request.UrlReferrer;
            UserAgent = request.UserAgent;
            UserHostAddress = request.UserHostAddress;
            UserHostName = request.UserHostName;

            ApplicationPath = request.ApplicationPath;
            AppRelativeCurrentExecutionFilePath = request.AppRelativeCurrentExecutionFilePath;
            CurrentExecutionFilePath = request.CurrentExecutionFilePath;
            FilePath = request.FilePath;
            Path = request.Path;
            PathInfo = request.PathInfo;
            PhysicalApplicationPath = request.PhysicalApplicationPath;
            PhysicalPath = request.PhysicalPath;
        }

        //// TODO: Add InputStream

        public IEnumerable<Cookie> Cookies { get; private set; }

        public CultureInfo CurrentUiCulture { get; private set; }

        public IEnumerable<PostedFile> Files { get; private set; }

        public IEnumerable<FormVariable> FormVariables { get; private set; }

        public IEnumerable<HeaderField> HeaderFields { get; private set; }

        public IEnumerable<QueryStringParameter> QueryString { get; private set; }

        public string RawUrl { get; private set; }

        public string RequestType { get; private set; }

        public Uri Url { get; private set; }

        public Uri UrlReferrer { get; private set; }

        public string UserAgent { get; private set; }

        public string UserHostAddress { get; private set; }

        public string UserHostName { get; private set; }

        public string ApplicationPath { get; private set; }

        public string AppRelativeCurrentExecutionFilePath { get; private set; }

        public string CurrentExecutionFilePath { get; private set; }

        public string FilePath { get; private set; }

        public string Path { get; private set; }

        public string PathInfo { get; private set; }

        public string PhysicalApplicationPath { get; private set; }

        public string PhysicalPath { get; private set; }

        private IEnumerable<Cookie> GetCookies(HttpRequestBase httpRequest, HttpServerUtilityBase server)
        {
            if (httpRequest != null)
            {
                var cookies = httpRequest.Cookies;

                if (cookies != null)
                {
                    foreach (var key in cookies.AllKeys)
                    {
                        var cookie = cookies[key];

                        if (cookie != null)
                        {
                            yield return new Cookie
                                {
                                    Name = cookie.Name,
                                    Value = server.UrlDecode(cookie.Value)
                                };
                        }
                    }
                }
            }
        }

        private IEnumerable<PostedFile> GetPostedFiles(HttpRequestBase httpRequest)
        {
            if (httpRequest != null)
            {
                HttpFileCollectionBase files = null;

                try
                {
                    files = httpRequest.Files;
                }
                catch (HttpException)
                {
                }

                if (files != null)
                {
                    foreach (var key in files.AllKeys)
                    {
                        var httpPostedFileBase = files[key];

                        if (httpPostedFileBase != null)
                        {
                            yield return new PostedFile
                            {
                                FileName = httpPostedFileBase.FileName,
                                ContentType = httpPostedFileBase.ContentType,
                                ContentLength = httpPostedFileBase.ContentLength
                            };
                        }
                    }
                }
            }
        }

        private IEnumerable<FormVariable> GetFormVariables(HttpRequestBase httpRequest)
        {
            if (httpRequest != null)
            {
                var formVariables = httpRequest.Form;

                if (formVariables != null)
                {
                    foreach (var key in formVariables.AllKeys)
                    {
                        yield return new FormVariable { Key = key, Value = SafeGetKeyValue(formVariables, key) };
                    }
                }
            }
        }

        private IEnumerable<HeaderField> GetHeaderFields(HttpRequestBase httpRequest)
        {
            if (httpRequest != null)
            {
                var headerFields = httpRequest.Headers;

                if (headerFields != null)
                {
                    foreach (var key in headerFields.AllKeys)
                    {
                        yield return new HeaderField { Key = key, Value = SafeGetKeyValue(headerFields, key) };
                    }
                }
            }
        }

        private IEnumerable<QueryStringParameter> GetQueryString(HttpRequestBase httpRequest)
        {
            if (httpRequest != null)
            {
                var queryString = httpRequest.QueryString;

                if (queryString != null)
                {
                    foreach (var key in queryString.AllKeys)
                    {
                        yield return new QueryStringParameter { Key = key, Value = SafeGetKeyValue(queryString, key) };
                    }
                }
            }
        }

        private static string SafeGetKeyValue(NameValueCollection collection, string key)
        {
            try
            {
                return collection[key];
            }
            catch (HttpRequestValidationException httpRequestValidationException)
            {
                return httpRequestValidationException.Message;
            }
        }

        public class Cookie
        {
            public string Name { get; set; }
            
            public string Value { get; set; }
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

        public class PostedFile
        {
            public string FileName { get; set; }
            
            public string ContentType { get; set; }
            
            public int ContentLength { get; set; }
        }

        public class QueryStringParameter
        {
            public string Key { get; set; }
            
            public string Value { get; set; }
        }
    }
}