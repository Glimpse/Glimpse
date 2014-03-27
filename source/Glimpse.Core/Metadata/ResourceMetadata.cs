using System.Collections.Generic; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class ResourceMetadata : IMetadata
    {
        public string Key
        {
            get { return "resources"; }
        }

        public object GetMetadata(IConfiguration configuration)
        {
            var logger = configuration.Logger;
            var resourceMetadata = new Dictionary<string, string>();

            var endpoint = configuration.ResourceEndpoint; 
            foreach (var resource in configuration.Resources)
            {
                var resourceKey = KeyCreator.Create(resource);
                if (resourceMetadata.ContainsKey(resourceKey))
                {
                    logger.Warn(Resources.GlimpseRuntimePersistMetadataMultipleResourceWarning, resource.Name);
                }

                resourceMetadata[resourceKey] = endpoint.GenerateUriTemplate(resource, configuration.EndpointBaseUri, logger);
            }

            return resourceMetadata;
        }
    }

    public static class ResourceExtensionExtension
    {
        public static IDictionary<string, string> GetResources(this IDictionary<string, object> metadata)
        {
            if (metadata != null)
            {
                var resources = metadata["resources"] as IDictionary<string, string>;
                if (resources != null)
                {
                    return resources;
                } 
            }

            return null;
        }
    }
}
