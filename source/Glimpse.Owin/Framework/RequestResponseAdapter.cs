using System.Collections.Generic;
using System.IO;
using System.Text;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Framework
{
    public class RequestResponseAdapter : IRequestResponseAdapter
    {
        private readonly IDictionary<string, object> environment;
        private readonly OwinRequest request;
        private readonly OwinResponse response;

        public RequestResponseAdapter(IDictionary<string, object> environment)
        {
            this.environment = environment;
            this.request = new OwinRequest(environment); // Merge RequestMetadata and requestResponseAdapter together?
            this.response = new OwinResponse(environment);
        }

        public IDataStore HttpRequestStore 
        {
#warning is it needed to store this? Since there will only be one instance of the OwinRequestResponseAdapter that will be created for each request
            get
            {
                const string key = "glimpse.RequestStore"; // Named by following the Owin key naming conventions documented at http://owin.org/spec/CommonKeys.html

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