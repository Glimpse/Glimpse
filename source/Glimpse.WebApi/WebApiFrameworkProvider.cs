using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using System.Web.Http.Controllers;

namespace Glimpse.WebApi
{
    public class WebApiFrameworkProvider : IFrameworkProvider
    {
        /// <summary>
        /// Wrapper around HttpContext.Current for testing purposes. Not for public use.
        /// </summary>
        private HttpRequestContext requestContext;

        public WebApiFrameworkProvider(ILogger logger)
        {
            Logger = logger;
        }

        public IDataStore HttpRequestStore
        {
            get { throw new NotImplementedException(); } // return new DictionaryDataStoreAdapter(Context.Items); }
        }

        public IDataStore HttpServerStore
        {
            get { throw new NotImplementedException(); } //return new HttpApplicationStateBaseDataStoreAdapter(Context.Application); }
        }

        public object RuntimeContext
        {
            get { return Context; }
        }

        public IRequestMetadata RequestMetadata
        {
            get { throw new NotImplementedException(); } // new RequestMetadata(Context); }
        }

        internal HttpRequestContext Context
        {
            get { return requestContext; }
            set { requestContext = value; }
        }

        private ILogger Logger { get; set; }


        public void SetHttpResponseHeader(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            throw new NotImplementedException();
        }

        public void SetCookie(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            throw new NotImplementedException();
        }

        public void WriteHttpResponse(byte[] content)
        {
            throw new NotImplementedException();
        }

        public void WriteHttpResponse(string content)
        {
            throw new NotImplementedException();
        }
    }
}