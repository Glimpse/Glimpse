namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines properties that describe the metadata associated with a request
    /// </summary>
    public interface IRequestMetadata
    {
        /// <summary>
        /// Gets the request URI.
        /// </summary>
        /// <value>The request URI.</value>
        string RequestUri { get; }

        /// <summary>
        /// Gets the request HTTP method.
        /// </summary>
        /// <value>The request HTTP method.</value>
        string RequestHttpMethod { get; }

        /// <summary>
        /// Gets the response status code.
        /// </summary>
        /// <value>The response status code.</value>
        int ResponseStatusCode { get; }

        /// <summary>
        /// Gets the type of the response content.
        /// </summary>
        /// <value>The type of the response content.</value>
        string ResponseContentType { get; }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        string IpAddress { get; }

        /// <summary>
        /// Gets a value indicating whether request is ajax.
        /// </summary>
        /// <value><c>true</c> if request is ajax; otherwise, <c>false</c>.</value>
        bool RequestIsAjax { get; }

        /// <summary>
        /// Gets the client id.
        /// </summary>
        /// <value>The client id.</value>
        string ClientId { get; }

        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Value of the Http cookie.</returns>
        string GetCookie(string name);

        /// <summary>
        /// Gets the HTTP header.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Value of the Http header.</returns>
        string GetHttpHeader(string name);
    }
}