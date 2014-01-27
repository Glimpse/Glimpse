using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents an inactive <see cref="GlimpseRequestContext"/>
    /// </summary>
    public class InactiveGlimpseRequestContext : GlimpseRequestContext
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="InactiveGlimpseRequestContext"/> type.
        /// </summary>
        public static InactiveGlimpseRequestContext Instance { get; private set; }

        static InactiveGlimpseRequestContext()
        {
            Instance = new InactiveGlimpseRequestContext();
        }

        private InactiveGlimpseRequestContext()
            : base(new Guid(), new RequestResponseAdapterStub(), string.Empty)
        {
        }

        /// <summary>
        /// Gets the active <see cref="RuntimePolicy"/> for this request
        /// </summary>
        public override RuntimePolicy ActiveRuntimePolicy
        {
            get { return RuntimePolicy.Off; }
        }

        /// <summary>
        /// Gets the <see cref="RequestHandlingMode"/> for this request
        /// </summary>
        public override RequestHandlingMode RequestHandlingMode
        {
            get { return RequestHandlingMode.Unhandled; }
        }

        private class RequestResponseAdapterStub : IRequestResponseAdapter
        {
            public RequestResponseAdapterStub()
            {
                HttpRequestStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>
                {
                    { Constants.RuntimePolicyKey, RuntimePolicy.Off }
                });

                RuntimeContext = new object();
                RequestMetadata = new RequestMetadataStub();
            }

            public IDataStore HttpRequestStore { get; private set; }
            public object RuntimeContext { get; private set; }
            public IRequestMetadata RequestMetadata { get; private set; }

            public void SetHttpResponseHeader(string name, string value)
            {
            }

            public void SetHttpResponseStatusCode(int statusCode)
            {
            }

            public void SetCookie(string name, string value)
            {
            }

            public void InjectHttpResponseBody(string htmlSnippet)
            {
            }

            public void WriteHttpResponse(byte[] content)
            {
            }

            public void WriteHttpResponse(string content)
            {
            }

            private class RequestMetadataStub : IRequestMetadata
            {
                public string RequestUri { get { return string.Empty; } }
                public string AbsolutePath { get { return string.Empty; } }
                public string RequestHttpMethod { get { return string.Empty; } }
                public int ResponseStatusCode { get { return 0; } }
                public string ResponseContentType { get { return string.Empty; } }
                public bool RequestIsAjax { get { return false; } }
                public string ClientId { get { return string.Empty; } }

                public string GetCookie(string name)
                {
                    return string.Empty;
                }

                public string GetHttpHeader(string name)
                {
                    return string.Empty;
                }
            }
        }
    }
}