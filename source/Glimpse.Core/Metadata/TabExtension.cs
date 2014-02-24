using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class TabExtension : IMetadataExtensions
    {
        public string Key
        {
            get { return "plugins"; }
        }

        public object Process(IGlimpseConfiguration configuration)
        {
            var logger = configuration.Logger;
            var tabMetadata = new Dictionary<string, object>();

            foreach (var tab in configuration.Tabs)
            {
                var metadataInstance = new Dictionary<string, object>();
                foreach (var extension in configuration.TabMetadataExtensions)
                {
                    try
                    {
                        var result = extension.ProcessTab(tab);
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
                    tabMetadata[GlimpseRuntime.CreateKey(tab)] = metadataInstance;
                }
            }

            return tabMetadata;
        }
    }
}
