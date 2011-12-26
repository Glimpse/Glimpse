using System.Collections.Generic;
using System.Text;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet
{
    public class HttpHandlerEndpointConfiguration:IGlimpseResourceEndpointConfiguration
    {
        public string GenerateUrl(string resourceName, string version)
        {
            return GenerateUrl(resourceName, version, null);
        }

        public string GenerateUrl(string resourceName, string version, IDictionary<string, string> parameters)
        {
            //TODO: Return properly rooted URL
            var stringBuilder = new StringBuilder(string.Format("Glimpse.axd?n={0}&v={1}", resourceName, version));

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    stringBuilder.Append(string.Format("&{0}={1}", parameter.Key, parameter.Value));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
