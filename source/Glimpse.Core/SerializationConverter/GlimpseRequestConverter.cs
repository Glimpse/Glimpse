using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="GlimpseRequest"/> representation's into a format suitable for serialization.
    /// </summary>
    public class GlimpseRequestConverter : SerializationConverter<GlimpseRequest>
    {
        /// <summary>
        /// Converts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An object suitable for serialization.</returns>
        public override object Convert(GlimpseRequest request)
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
                           { "data", request.TabData },
                           { "metadata", request.Metadata },
                           { "hud", request.DisplayData },
                           { "userAgent", request.UserAgent },
                       };
        }
    }
}