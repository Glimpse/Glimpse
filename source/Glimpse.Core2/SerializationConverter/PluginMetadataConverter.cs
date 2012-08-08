using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.SerializationConverter
{
    public class PluginMetadataConverter : SerializationConverter<PluginMetadata>
    {
        public override object Convert(PluginMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           {"documentationUri", metadata.DocumentationUri}
                           //Don't think the client needs "HasMetadata", so leaving it out
                       };
        }
    }
}