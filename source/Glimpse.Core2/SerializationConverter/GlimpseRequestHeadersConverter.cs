using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.SerializationConverter
{
    public class GlimpseRequestHeadersConverter:SerializationConverter<GlimpseRequestHeaders>
    {
        public override IDictionary<string, object> Convert(GlimpseRequestHeaders request)
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
                           {"userAgent", request.UserAgent},
                       };
        }
    }
}