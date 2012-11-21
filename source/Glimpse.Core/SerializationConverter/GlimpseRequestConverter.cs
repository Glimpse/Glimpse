using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    public class GlimpseRequestConverter : SerializationConverter<GlimpseRequest>
    {
        public override object Convert(GlimpseRequest request)
        {
            return new Dictionary<string, object>
                       {
                           { "clientId", request.ClientId },
                           { "dateTime", request.DateTime },
                           { "duration", request.Duration.ToString("0.## ms") },
                           { "parentRequestId", request.ParentRequestId },
                           { "requestId", request.RequestId },
                           { "isAjax", request.RequestIsAjax },
                           { "method", request.RequestHttpMethod },
                           { "uri", request.RequestUri },
                           { "contentType", request.ResponseContentType },
                           { "statusCode", request.ResponseStatusCode },
                           { "data", request.PluginData },
                           { "userAgent", request.UserAgent },
                       };
        }
    }
}