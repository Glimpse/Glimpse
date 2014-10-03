using System;
using System.Collections.Generic; 

namespace Glimpse.Core.Framework
{
    public class MetadataProvider
    {
        protected IReadOnlyConfiguration Configuration { get; set; }

        public MetadataProvider(IReadOnlyConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDictionary<string, object> GetRequestMetadata(IGlimpseRequestContext requestContext)
        {
            var logger = Configuration.Logger;
            var metadata = new Dictionary<string, object>();

            foreach (var extension in Configuration.InstanceMetadata)
            {
                try
                {
                    var result = extension.GetInstanceMetadata(Configuration, requestContext);
                    if (result != null)
                    {
                        metadata[extension.Key] = result;
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.ExecuteInstanceMetadataExtensionsError, exception, extension.GetType());
                }
            }

            return metadata;
        }
    }
}
