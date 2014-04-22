using System;
using System.Collections.Generic;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class TabMetadata : IMetadata
    {
        public string Key
        {
            get { return "plugins"; }
        }

        public object GetMetadata(IConfiguration configuration)
        {
            var logger = configuration.Logger;
            var tabMetadata = new Dictionary<string, object>();

            foreach (var tab in configuration.Tabs)
            {
                var metadataInstance = new Dictionary<string, object>();
                foreach (var extension in configuration.TabMetadata)
                {
                    try
                    {
                        var result = extension.GetTabMetadata(tab);
                        if (result != null)
                        {
                            metadataInstance[extension.Key] = result;
                        }
                    }
                    catch (Exception exception)
                    {
                        logger.Error(Resources.ExecuteTabMetadataExtensionsError, exception, extension.GetType());
                    }
                }

                if (metadataInstance.Count > 0)
                {
                    tabMetadata[KeyCreator.Create(tab)] = metadataInstance;
                }
            }

            return tabMetadata;
        }
    }
}
