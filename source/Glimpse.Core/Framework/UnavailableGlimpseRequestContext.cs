using System;
using System.Runtime.Serialization;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents an unavailable <see cref="IGlimpseRequestContext"/>
    /// </summary>
    public class UnavailableGlimpseRequestContext : IGlimpseRequestContext
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="UnavailableGlimpseRequestContext"/> type.
        /// </summary>
        public static UnavailableGlimpseRequestContext Instance { get; private set; }

        static UnavailableGlimpseRequestContext()
        {
            Instance = new UnavailableGlimpseRequestContext();
        }

        private UnavailableGlimpseRequestContext()
        {
            GlimpseRequestId = new Guid();
            RequestResponseAdapter = new RequestResponseAdapterStub();
            RequestStore = new DataStoreStub();
        }

        /// <summary>
        /// Gets a default <see cref="Guid"/> representing the unavailable request
        /// </summary>
        public Guid GlimpseRequestId { get; private set; }

        /// <summary>
        /// Gets the <see cref="IRequestResponseAdapter"/> of the unavailable request
        /// </summary>
        public IRequestResponseAdapter RequestResponseAdapter { get; private set; }

        /// <summary>
        /// Gets the <see cref="IDataStore"/> for the unavailable request
        /// </summary>
        public IDataStore RequestStore { get; private set; }

        /// <summary>
        /// Gets the active <see cref="RuntimePolicy"/> for the unavailable request
        /// </summary>
        public RuntimePolicy CurrentRuntimePolicy
        {
            get { return RuntimePolicy.Off; }
            set { throw new GlimpseRequestContextUnavailableException(); }
        }

        /// <summary>
        /// Gets the <see cref="RequestHandlingMode"/> for the unavailable request
        /// </summary>
        public RequestHandlingMode RequestHandlingMode
        {
            get { return RequestHandlingMode.Unhandled; }
        }

        /// <summary>
        /// A custom exception thrown when 
        /// </summary>
        public class GlimpseRequestContextUnavailableException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GlimpseRequestContextUnavailableException" /> class.
            /// </summary>
            public GlimpseRequestContextUnavailableException()
                : this("There is no Glimpse request context available. Make sure to check that the GlimpseRuntime.Instance.CurrentRequestContext.CurrentRuntimePolicy != RuntimePolicy.Off before accessing any further details.")
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GlimpseRequestContextUnavailableException" /> class.
            /// </summary>
            /// <param name="message">The message.</param>
            public GlimpseRequestContextUnavailableException(string message)
                : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GlimpseRequestContextUnavailableException" /> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="innerException">The inner exception.</param>
            public GlimpseRequestContextUnavailableException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GlimpseRequestContextUnavailableException" /> class.
            /// </summary>
            /// <param name="info">The info.</param>
            /// <param name="context">The context.</param>
            public GlimpseRequestContextUnavailableException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        private class RequestResponseAdapterStub : IRequestResponseAdapter
        {
            public object RuntimeContext
            {
                get { throw new GlimpseRequestContextUnavailableException(); }
            }

            public IRequestMetadata RequestMetadata
            {
                get { throw new GlimpseRequestContextUnavailableException(); }
            }

            public void SetHttpResponseHeader(string name, string value)
            {
                throw new GlimpseRequestContextUnavailableException();
            }

            public void SetHttpResponseStatusCode(int statusCode)
            {
                throw new GlimpseRequestContextUnavailableException();
            }

            public void SetCookie(string name, string value)
            {
                throw new GlimpseRequestContextUnavailableException();
            }

            public void InjectHttpResponseBody(string htmlSnippet)
            {
                throw new GlimpseRequestContextUnavailableException();
            }

            public void WriteHttpResponse(byte[] content)
            {
                throw new GlimpseRequestContextUnavailableException();
            }

            public void WriteHttpResponse(string content)
            {
                throw new GlimpseRequestContextUnavailableException();
            }
        }

        private class DataStoreStub : IDataStore
        {
            public object Get(string key)
            {
                return null;
            }

            public void Set(string key, object value)
            {
            }

            public bool Contains(string key)
            {
                return false;
            }
        }
    }
}