using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public abstract class ResourceEndpointConfiguration
    {
        public string GenerateUri(IResource resource, ILogger logger, IDictionary<string, string> requestTokenValues)
        {
            Contract.Requires<ArgumentNullException>(resource != null, "resource");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(requestTokenValues != null, "requestTokenValues");
            Contract.Ensures(Contract.Result<string>() != null);

            var parmeters = new Dictionary<string, string>();

            try
            {
                foreach (var key in resource.ParameterKeys)
                {
                    var value = requestTokenValues.ContainsKey(key)
                                    ? requestTokenValues[key]
                                    : string.Format("{{{0}}}", key);
                    parmeters.Add(key, value);
                }
            }
            catch(Exception exception)
            {
                logger.Warn(string.Format(Resources.GenerateUriParameterKeysWarning, resource.GetType()), exception);
            }

            string result = null;
            try
            {
                result = GenerateUri(resource.Name, parmeters, logger);
            }
            catch(Exception exception)
            {
                logger.Error(string.Format(Resources.GenerateUriExecutionError, GetType()), exception);
            }

            if (result != null)
                return result;

            return string.Empty;
        }

        protected abstract string GenerateUri(string resourceName, IEnumerable<KeyValuePair<string, string>> parameters, ILogger logger);
    }
}
