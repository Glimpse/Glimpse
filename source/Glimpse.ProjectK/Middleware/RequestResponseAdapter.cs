using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Microsoft.AspNet.Http;

namespace Glimpse.ProjectK.Middleware
{
    public class RequestResponseAdapter : IRequestResponseAdapter
    {
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
    }
}