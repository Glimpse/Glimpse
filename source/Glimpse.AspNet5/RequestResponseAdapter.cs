using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Microsoft.AspNet.Http;
using Glimpse.Core;

namespace Glimpse.AspNet5.Middleware
{
    public class RequestResponseAdapter : IRequestResponseAdapter
    {
        private readonly HttpContext context;
        private readonly HttpRequest request;
        private readonly HttpResponse response;

        public RequestResponseAdapter(HttpContext context)
        {
            this.context = context;
            this.request = context.Request;
            this.response = context.Response;
        }

        public IDataStore HttpRequestStore
        {
            get
            {
                const string key = "glimpse.RequestStore";

                if (context.Items.ContainsKey(key))
                {
                    return (IDataStore)context.Items[key];
                }

                var result = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
                context.Items.Add(key, result);
                return result;
            }
        }

        public object RuntimeContext
        {
            get { return context; }
        }

        public Stream OutputStream
        {
            get
            {
                return response.Body;
            }

            set
            {
                Guard.ArgumentNotNull("value", value);
                response.Body = value;
            }
        }

#warning TODO find a better way to "know" what the content encoding is (needed by the wrapping output stream)
        public Encoding ResponseEncoding
        {
            get { return Encoding.UTF8; }
        }

        public IRequestMetadata RequestMetadata
        {
            get { return new RequestMetadata(context); }
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            response.Headers[name] = value;
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            response.StatusCode = statusCode;
        }

        public void SetCookie(string name, string value)
        {
            response.Cookies.Append(name, value);
        }
         
        public void WriteHttpResponse(byte[] content)
        {
            response.Body.WriteAsync(content, 0, content.Length);
        }

        public void WriteHttpResponse(string content)
        {
            response.WriteAsync(content);
        }
        /*
        private HttpContext context;

        public RequestResponseAdapter(HttpContext context)
        {
            this.context = context;
        }

        public IDataStore HttpRequestStore 
        {
            get
            {
                const string key = "glimpse.RequestStore";

                if (context.Items.ContainsKey(key))
                {
                    return (IDataStore)context.Items[key];
                }
                 
                var result = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
                context.Items.Add(key, result);
                return result;
            }
        }

        public object RuntimeContext 
        {
            get { return context; }
        }

        public IRequestMetadata RequestMetadata 
        {
            get { return new RequestMetadata(context); }
        }

        public Stream OutputStream
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Encoding ResponseEncoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            context.Response.Headers[name] = value;
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            context.Response.StatusCode = statusCode;
        }

        public void SetCookie(string name, string value)
        {
            context.Response.Cookies.Append(name, value);
        }

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            // Hack: doing nothing because this has been temporarily moved to HeadMiddlewear
        }

        public void WriteHttpResponse(byte[] content)
        {
            context.Response.Body.WriteAsync(content, 0, content.Length);
        }

        public void WriteHttpResponse(string content)
        {
            context.Response.WriteAsync(content);
        }
        */
    }
}