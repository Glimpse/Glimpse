using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="GlimpseRequestHeaders"/> representation's into a format suitable for serialization.
    /// </summary>
    public class GlimpseRequestHeadersConverter : SerializationConverter<GlimpseRequestHeaders>
    {
        /// <summary>
        /// Converts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An object suitable for serialization.</returns>
        public override object Convert(GlimpseRequestHeaders request)
        {
            return new Dictionary<string, object>
                       {
                           { "clientId", request.ClientId },
                           { "dateTime", request.DateTime },
                           { "duration", request.Duration },
                           { "parentRequestId", request.ParentRequestId },
                           { "requestId", request.RequestId },
                           { "isAjax", request.RequestIsAjax },
                           { "method", request.RequestHttpMethod },
                           { "uri", request.RequestUri },
                           { "contentType", request.ResponseContentType },
                           { "statusCode", request.ResponseStatusCode },
                           { "userAgent", request.UserAgent },
                       };
        }
    }
}