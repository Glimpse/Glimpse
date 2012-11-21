using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    public class PluginMetadataConverter : SerializationConverter<PluginMetadata>
    {
        public override object Convert(PluginMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           { "documentationUri", metadata.DocumentationUri },
                           { "layout", metadata.Layout }
                       };
        }
    }
}