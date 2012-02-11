using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.SerializationConverter
{
    public class GlimpseMetadataConverter:SerializationConverter<GlimpseMetadata>
    {
        public override IDictionary<string, object> Convert(GlimpseMetadata metadata)
        {
            return new Dictionary<string, object>
                       {
                           {"clientId", metadata.ClientId},
                           {"dateTime", metadata.DateTime},
                           {"duration", metadata.Duration},
                           {"parentRequestId", metadata.ParentRequestId},
                           {"requestId", metadata.RequestId},
                           {"isAjax", metadata.RequestIsAjax},
                           {"method", metadata.RequestHttpMethod},
                           {"uri", metadata.RequestUri},
                           {"contentType", metadata.ResponseContentType},
                           {"statusCode", metadata.ResponseStatusCode},
                           {"plugins", metadata.PluginData},
                           {"userAgent", metadata.UserAgent}
                       };
        }
    }
}