using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    public class GlimpseRequestHeadersConverter:SerializationConverter<GlimpseRequestHeaders>
    {
        public override object Convert(GlimpseRequestHeaders request)
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