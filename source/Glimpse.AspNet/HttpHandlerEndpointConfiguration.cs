using System.Collections.Generic;
using System.Text;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class HttpHandlerEndpointConfiguration:ResourceEndpointConfiguration
    {
        protected override string GenerateUriTemplate(string resourceName, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            //TODO: Return properly rooted URL
            var stringBuilder = new StringBuilder(string.Format(@"/Glimpse.axd?n={0}", resourceName));

            var requiredParams = parameters.Where(p => p.IsRequired);
            foreach (var parameter in requiredParams)
            {
                stringBuilder.Append(string.Format("&{0}={{{1}}}", parameter.Name, parameter.Name));
            }

            var optionalParams = parameters.Except(requiredParams).Select(p=>p.Name).ToArray();

            //Format based on Form-style query continuation from RFC6570: http://tools.ietf.org/html/rfc6570#section-3.2.9
            if(optionalParams.Any())
                stringBuilder.Append(string.Format("{{&{0}}}", string.Join(",", optionalParams)));

            return stringBuilder.ToString();
        }
    }
}
