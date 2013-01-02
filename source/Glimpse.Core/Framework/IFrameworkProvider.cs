using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement a Framework Provider
    /// </summary>
    /// <remarks>
    /// Required by any different Framework Provider - i.e. ASP.NET, Self Hosted WebAPI, 
    /// NancyFX, etc. As an example implementation see Glimpse.AspNet.AspNetFrameworkProvider
    /// as reference implementation.
    /// </remarks>
    public interface IFrameworkProvider
    {
        /// <summary>
        /// Gets the HTTP request store.
        /// </summary>
        /// <value>The HTTP request store.</value>
        /// <remarks>
        /// Default expectation is that a new instance is returned on each usage. This 
        /// the local request storage (i.e. <see cref="System.Web.HttpContext.Items"/>).
        /// </remarks>
        IDataStore HttpRequestStore { get; }

        /// <summary>
        /// Gets the HTTP server store.
        /// </summary>
        /// <value>The HTTP server store.</value>
        /// <remarks>
        /// Default expectation is that a new instance is returned on each usage. This 
        /// the local request storage (i.e. <see cref="System.Web.HttpContext.Application"/>).
        /// </remarks>
        IDataStore HttpServerStore { get; }

        /// <summary>
        /// Gets the runtime context.
        /// </summary>
        /// <value>The runtime context.</value>
        /// <remarks>
        /// Instance of the underlying context that the Framework Provider uses 
        /// (i.e. <see cref="System.Web.HttpContext.Current"/>). 
        /// </remarks>
        object RuntimeContext { get; }

        /// <summary>
        /// Gets the request metadata.
        /// </summary>
        /// <value>The request metadata.</value>
        /// <remarks>
        /// Default expectation is that a new instance is returned on each usage. Provides 
        /// access to a request metadata abstraction.
        /// </remarks>
        IRequestMetadata RequestMetadata { get; }

        /// <summary>
        /// Sets the HTTP response header.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetHttpResponseHeader(string name, string value);

        /// <summary>
        /// Sets the HTTP response status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        void SetHttpResponseStatusCode(int statusCode);

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetCookie(string name, string value);

        /// <summary>
        /// Injects the HTTP response body.
        /// </summary>
        /// <param name="htmlSnippet">The HTML snippet.</param>
        /// <remarks>
        /// Inserts the given html snippet into the html document just before the end body tag.
        /// </remarks>
        void InjectHttpResponseBody(string htmlSnippet);

        /// <summary>
        /// Writes the HTTP response.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <remarks>
        /// Used by the resource infrastructure to output binary content (i.e. embedded content,
        /// images, etc).
        /// <seealso cref="Glimpse.Core.Extensibility.IResourceResult"/>
        /// <seealso cref="Glimpse.Core.Extensibility.IResource"/>
        /// </remarks>
        void WriteHttpResponse(byte[] content);

        /// <summary>
        /// Writes the HTTP response.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <remarks>
        /// Used by the resource infrastructure to output string content (i.e. generated strings,
        /// JSON objects, etc).
        /// <seealso cref="IResourceResult"/>
        /// </remarks>
        void WriteHttpResponse(string content);
    }
}
