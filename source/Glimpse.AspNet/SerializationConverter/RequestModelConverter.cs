using System.Collections.Generic;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.SerializationConverter
{
    public class RequestModelConverter : SerializationConverter<RequestModel>
    {
        public override object Convert(RequestModel request)
        {
            return new Dictionary<string, object>
                       {
                           //TODO: Leverage Kristoffer Ahl's fluent interface for transformation into a table and formatting
                           {"Cookies", request.Cookies.ToTable()},
                           //TODO: Leverage Kristoffer Ahl's fluent interface for transformation into a table and formatting
                           {"Query String", request.QueryString.ToTable()},
                           {"Url", request.Url.ToString()},
                           {"Url Referrer", request.UrlReferrer.OrNull()},
                           {"App Relative Current Execution File Path", request.AppRelativeCurrentExecutionFilePath},
                           {"Application Path", request.ApplicationPath},
                           {"Current Execution File Path", request.CurrentExecutionFilePath},
                           {"Current UI Culture", request.CurrentUICulture},
                           {"File Path", request.FilePath},
                           {"Path", request.Path},
                           {"Path Info", request.PathInfo},
                           {"Physical Application Path", request.PhysicalApplicationPath},
                           {"Physical Path", request.PhysicalPath},
                           {"Raw Url", request.RawUrl},
                           {"User Agent", request.UserAgent},
                           {"User Host Address", request.UserHostAddress},
                           {"User Host Name", request.UserHostName},
                       };
        }
    }
}