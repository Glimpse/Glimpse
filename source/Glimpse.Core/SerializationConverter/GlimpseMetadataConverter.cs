using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    public class GlimpseMetadataConverter : SerializationConverter<GlimpseMetadata>
    {
        public override object Convert(GlimpseMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           { "version", metadata.Version },
                           { "plugins", metadata.Plugins },
                           { "resources", metadata.Resources },
                       };
        }
    }
}