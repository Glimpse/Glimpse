using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents a Glimpse resource endpoint configuration
    /// </summary>
    public interface IResourceEndpointConfiguration
    {
        /// <summary>
        /// Generates the URI template.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A Uri template a client can expand to invoke a resource.</returns>
        /// <exception cref="System.ArgumentNullException">Throws and exception if <paramref name="resource"/> or <paramref name="logger"/> is <c>null</c>.</exception>
        string GenerateUriTemplate(IResource resource, string baseUri, ILogger logger);

        /// <summary>
        /// Checks whether the given <paramref name="requestUri"/> is a request for a Glimpse <see cref="IResource"/> or not
        /// </summary>
        /// <param name="requestUri">The request URI to check</param>
        /// <param name="endpointBaseUri">The endpoint base URI to check against</param>
        /// <returns>Boolean indicating whether a Glimpse <see cref="IResource"/> request is made or not</returns>
        bool IsResourceRequest(Uri requestUri, string endpointBaseUri);
    }
}