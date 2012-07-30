using System;
using System.Collections.Generic;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public abstract class ResourceEndpointConfiguration
    {
        public string GenerateUriTemplate(IResource resource, ILogger logger)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            if (logger == null) throw new ArgumentNullException("logger");

            string result = null;
            try
            {
                result = GenerateUriTemplate(resource.Name, resource.Parameters, logger);
            }
            catch(Exception exception)
            {
                logger.Error(Resources.GenerateUriExecutionError, exception, GetType());
            }

            if (result != null)
                return result;

            return string.Empty;
        }

        protected abstract string GenerateUriTemplate(string resourceName, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger);
    }
}
