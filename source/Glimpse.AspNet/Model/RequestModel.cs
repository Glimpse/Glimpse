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

            CurrentUiCulture = Thread.CurrentThread.CurrentUICulture;
            ApplicationPath = request.ApplicationPath;
            AppRelativeCurrentExecutionFilePath = request.AppRelativeCurrentExecutionFilePath;
            CurrentExecutionFilePath = request.CurrentExecutionFilePath;
            FilePath = request.FilePath;
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

            Cookies = GetCookies(request.Cookies, context.Server);
            QueryString = GetQueryString(request.QueryString);
        }

        //// TODO: Add Form
        //// TODO: Add InputStream

        public CultureInfo CurrentUiCulture { get; private set; }
        
        public string ApplicationPath { get; private set; }
        
        public string AppRelativeCurrentExecutionFilePath { get; private set; }
        
        public string CurrentExecutionFilePath { get; private set; }
        
        public string FilePath { get; private set; }
        
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

        private IEnumerable<Cookie> GetCookies(HttpCookieCollection cookies, HttpServerUtilityBase server)
        {
            var result = new List<Cookie>();

            foreach (var key in cookies.AllKeys)
            {
                var cookie = cookies[key];

                result.Add(new Cookie
                        {
                            Name = cookie.Name,
                            Path = cookie.Path,
                            IsSecure = cookie.Secure,
                            Value = server.UrlDecode(cookie.Value)
                        });
            }

            return result;
        }

        private IEnumerable<QueryStringParameter> GetQueryString(NameValueCollection queryString)
        {
            foreach (var key in queryString.AllKeys)
            {
                yield return new QueryStringParameter { Key = key, Value = queryString[key] };
            }
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