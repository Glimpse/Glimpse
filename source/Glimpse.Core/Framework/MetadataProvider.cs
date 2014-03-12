using System;
using System.Collections.Generic; 

namespace Glimpse.Core.Framework
{
    public class MetadataProvider
    {
        protected IReadonlyConfiguration Configuration { get; set; }

        public MetadataProvider(IReadonlyConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDictionary<string, object> GetMetadata()
        {
            var logger = Configuration.Logger;
            var metadata = new Dictionary<string, object>();

            foreach (var extension in Configuration.Metadata)
            {
                try
                {
                    var result = extension.GetMetadata(Configuration);
                    if (result != null)
                    {
                        metadata[extension.Key] = result;
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.ExecuteMetadataExtensionsError, exception, extension.GetType());
                }
            }

            return metadata;
        }

        public void SaveMetadata()
        { 
            Configuration.PersistenceStore.SaveMetadata(GetMetadata());
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
