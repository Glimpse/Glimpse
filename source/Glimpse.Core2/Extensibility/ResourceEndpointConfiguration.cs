using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Glimpse.Core2.Extensibility
{
    public abstract class ResourceEndpointConfiguration
    {
        public string GenerateUri(IResource resource, IDictionary<string, string> requestTokenValues)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            var parmeters = new Dictionary<string, string>();

            //TODO: Handle possible exceptions/null when calling .ParameterKeys
            foreach (var key in resource.ParameterKeys)
            {
                var value = requestTokenValues.ContainsKey(key) ? requestTokenValues[key] : string.Format("{{{0}}}", key);
                parmeters.Add(key, value);
            }

            //TODO: Handle possible exception/null when calling .GenerateUri(...)
            var result = GenerateUri(resource.Name, parmeters);

            if (result != null)
                return result;

            return string.Empty;
        }

        protected abstract string GenerateUri(string resourceName, IEnumerable<KeyValuePair<string, string>> parameters);
    }
}
