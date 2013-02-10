using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement a resource endpoint configuration.
    /// </summary>
    public abstract class ResourceEndpointConfiguration
    {
        /// <summary>
        /// Generates the URI template.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A Uri template a client can expand to invoke a resource.</returns>
        /// <exception cref="System.ArgumentNullException">Throws and exception if <paramref name="resource"/> or <paramref name="logger"/> is <c>null</c>.</exception>
        public string GenerateUriTemplate(IResource resource, string baseUri, ILogger logger)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            string result = null;
            try
            {
                result = GenerateUriTemplate(resource.Name, baseUri, resource.Parameters, logger);
            }
            catch (Exception exception)
            {
                logger.Error(Resources.GenerateUriExecutionError, exception, GetType());
            }

            if (result != null)
            {
                return result;
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates the URI template.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A Uri template a client can expand to invoke a resource.</returns>
        protected abstract string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger);
    }
}
