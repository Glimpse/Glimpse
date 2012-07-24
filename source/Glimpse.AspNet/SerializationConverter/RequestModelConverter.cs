using System.Collections.Generic;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.SerializationConverter
{
    public class RequestModelConverter : SerializationConverter<RequestModel>
    {
        public override IDictionary<string, object> Convert(RequestModel request)
        {
            return new Dictionary<string, object>
                       {
                           //TODO: Leverage Kristoffer Ahl's fluent interface for transformation into a table and formatting
                           {"Cookies", request.Cookies.ToTable()},
                           //TODO: Leverage Kristoffer Ahl's fluent interface for transformation into a table and formatting
                           {"Query String", request.QueryString.ToTable()},
                           {"Url", request.Url.ToString()},
                           {"Url Referrer", request.UrlReferrer == null ? null : request.UrlReferrer.ToString()},
                           //TODO: Create .ToGlimpseString(this Uri)
                           {"appRelativeCurrentExecutionFilePath", request.AppRelativeCurrentExecutionFilePath},
                           {"applicationPath", request.ApplicationPath},
                           {"currentExecutionFilePath", request.CurrentExecutionFilePath},
                           {"currentUICulture", request.CurrentUICulture},
                           {"filePath", request.FilePath},
                           {"path", request.Path},
                           {"pathInfo", request.PathInfo},
                           {"physicalApplicationPath", request.PhysicalApplicationPath},
                           {"physicalPath", request.PhysicalPath},
                           {"rawUrl", request.RawUrl},
                           {"userAgent", request.UserAgent},
                           {"userHostAddress", request.UserHostAddress},
                           {"userHostName", request.UserHostName},
                       };
        }
    }
}