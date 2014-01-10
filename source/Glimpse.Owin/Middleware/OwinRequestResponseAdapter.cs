using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Middleware
{
    public class OwinRequestResponseAdapter : IRequestResponseAdapter
    {
        private IDictionary<string, object> environment;
        private OwinRequest request;
        private OwinResponse response;

        public OwinRequestResponseAdapter(IDictionary<string, object> environment)
        {
            this.environment = environment;
            this.request = new OwinRequest(environment); // Merge RequestMetadata and requestResponseAdapter together?
            this.response = new OwinResponse(environment);
        }

        public IDataStore HttpRequestStore 
        {
            get
            {
                const string key = "glimpse.requestStore";

                if (environment.ContainsKey(key))
                {
                    return (IDataStore)environment[key];
                }

                var result = new DictionaryDataStoreAdapter(new Dictionary<string, object>());
                environment.Add(key, result);
                return result;
            }
        }

        public object RuntimeContext 
        {
            get { return environment; }
        }

        public IRequestMetadata RequestMetadata 
        {
            get { return new RequestMetadata(request, response); }
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

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            // Hack: doing nothing because this has been temporarily moved to HeadMiddlewear
        }

        public void WriteHttpResponse(byte[] content)
        {
            response.Write(content);
        }

        public void WriteHttpResponse(string content)
        {
            response.Write(content);
        }
    }
}