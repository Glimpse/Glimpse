using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.SerializationConverter
{
    public class GlimpseRequestConverter:SerializationConverter<GlimpseRequest>
    {
        public override IDictionary<string, object> Convert(GlimpseRequest request)
        {
            return new Dictionary<string, object>
                       {
                           {"clientId", request.ClientId},
                           {"dateTime", request.DateTime},
                           {"duration", request.Duration},
                           {"parentRequestId", request.ParentRequestId},
                           {"requestId", request.RequestId},
                           {"isAjax", request.RequestIsAjax},
                           {"method", request.RequestHttpMethod},
                           {"uri", request.RequestUri},
                           {"contentType", request.ResponseContentType},
                           {"statusCode", request.ResponseStatusCode},
                           {"data", request.PluginData},
                           {"userAgent", request.UserAgent},
                           {"clientName", ""} //TODO Nik needs to fix this up
                       };
        }
    }
}