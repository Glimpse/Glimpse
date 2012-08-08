using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.SerializationConverter
{
    public class GlimpseMetadataConverter : SerializationConverter<GlimpseMetadata>
    {
        public override object Convert(GlimpseMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           {"version", metadata.Version},
                           {"plugins", metadata.Plugins},
                           {"resources", metadata.Resources}
                       };
        }
    }
}