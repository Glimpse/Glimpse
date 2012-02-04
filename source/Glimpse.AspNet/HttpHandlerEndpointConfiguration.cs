using System.Collections.Generic;
using System.Text;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class HttpHandlerEndpointConfiguration:ResourceEndpointConfiguration
    {
        protected override string GenerateUri(string resourceName, IEnumerable<KeyValuePair<string, string>> parameters, ILogger logger)
        {
            //TODO: Return properly rooted URL
            var stringBuilder = new StringBuilder(string.Format(@"/Glimpse.axd?n={0}", resourceName));

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
