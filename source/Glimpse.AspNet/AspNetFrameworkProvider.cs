using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core;

namespace Glimpse.AspNet
{
    public class AspNetFrameworkProvider : IFrameworkProvider
    {
        /// <summary>
        /// Wrapper around HttpContext.Current for testing purposes. Not for public use.
        /// </summary>
        private HttpContextBase context;

        internal HttpContextBase Context
        {
            get { return context ?? new HttpContextWrapper(HttpContext.Current); }
            set { context = value; }
        }

        public IDataStore HttpRequestStore
        {
            get { return new DictionaryDataStoreAdapter(Context.Items); }
        }

        public IDataStore HttpServerStore
        {
            get { return new HttpApplicationStateBaseDataStoreAdapter(Context.Application); }
        }

        public object RuntimeContext
        {
            get { return Context; }
        }

        public IRequestMetadata RequestMetadata
        {
            get { return new RequestMetadata(Context); }
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            Context.Response.AppendHeader(name, value);
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            Context.Response.StatusCode = statusCode;
            Context.Response.StatusDescription = null;
        }

        public void SetCookie(string name, string value)
        {
            Context.Response.Cookies.Add(new HttpCookie(name, value));
        }

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            var response = Context.Response;
            response.Filter = new PreBodyTagFilter(htmlSnippet, response.Filter, response.ContentEncoding);
        }

        public void WriteHttpResponse(byte[] content)
        {
            Context.Response.BinaryWrite(content);
        }

        public void WriteHttpResponse(string content)
        {
            Context.Response.Write(content);
        }
    }
}