using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public abstract class ResourceEndpointConfiguration
    {
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

        protected abstract string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger);
    }
}
