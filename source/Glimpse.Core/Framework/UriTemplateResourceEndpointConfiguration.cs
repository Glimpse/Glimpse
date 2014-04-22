using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class UriTemplateResourceEndpointConfiguration : ResourceEndpointConfiguration
    {
        public const string DefaultResourceNameKey = "n";

        public UriTemplateResourceEndpointConfiguration() : this(DefaultResourceNameKey)
        {
        }

        public UriTemplateResourceEndpointConfiguration(string resourceNameKey)
        {
            ResourceNameKey = resourceNameKey;
        }

        public string ResourceNameKey { get; set; }

        protected override string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            var stringBuilder = new StringBuilder(string.Format(@"{0}?{1}={2}", baseUri, ResourceNameKey, resourceName));

            var requiredParams = parameters.Where(p => p.IsRequired);
            foreach (var parameter in requiredParams)
            {
                stringBuilder.Append(string.Format("&{0}={{{1}}}", parameter.Name, parameter.Name));
            }

            var optionalParams = parameters.Except(requiredParams).Select(p => p.Name).ToArray();

            // Format based on Form-style query continuation from RFC6570: http://tools.ietf.org/html/rfc6570#section-3.2.9
            if (optionalParams.Any())
            {
                stringBuilder.Append(string.Format("{{&{0}}}", string.Join(",", optionalParams)));
            }

            return stringBuilder.ToString();
        }
    }
}