using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class RequestModelConverter : SerializationConverter<RequestModel>
    {
        public override object Convert(RequestModel request)
        {
            var root = new TabObject();
            root.AddRow().Key("Cookies").Value(BuildCookies(request.Cookies));
            root.AddRow().Key("Query String").Value(BuildQueryString(request.QueryString));
            root.AddRow().Key("Url").Value(request.Url.ToString());
            root.AddRow().Key("Url Referrer").Value(request.UrlReferrer.ToStringOrDefault());
            root.AddRow().Key("App Relative Current Execution File Path").Value(request.AppRelativeCurrentExecutionFilePath);
            root.AddRow().Key("Application Path").Value(request.ApplicationPath);
            root.AddRow().Key("Current Execution File Path").Value(request.CurrentExecutionFilePath);
            root.AddRow().Key("Current UI Culture").Value(request.CurrentUiCulture);
            root.AddRow().Key("File Path").Value(request.FilePath);
            root.AddRow().Key("Form Variables").Value(BuildFormVariables(request.FormVariables));
            root.AddRow().Key("Path").Value(request.Path);
            root.AddRow().Key("Path Info").Value(request.PathInfo);
            root.AddRow().Key("Physical Application Path").Value(request.PhysicalApplicationPath);
            root.AddRow().Key("Physical Path").Value(request.PhysicalPath);
            root.AddRow().Key("Raw Url").Value(request.RawUrl);
            root.AddRow().Key("User Agent").Value(request.UserAgent);
            root.AddRow().Key("User Host Address").Value(request.UserHostAddress);
            root.AddRow().Key("User Host Name").Value(request.UserHostName);

            return root.Build();
        }

        public object BuildCookies(IEnumerable<RequestModel.Cookie> cookies)
        {
            if (!cookies.Any())
            {
                return null;
            }

            var result = new TabSection("Name", "Value", "Path", "Secure");
            foreach (var cookie in cookies)
            {
                result.AddRow().Column(cookie.Name).Column(cookie.Value).Column(cookie.Path).Column(cookie.IsSecure);
            }

            return result;
        }

        private object BuildFormVariables(IEnumerable<RequestModel.FormVariable> formVariables)
        {
            if (!formVariables.Any())
            {
                return null;
            }
            return formVariables;
        }

        public object BuildQueryString(IEnumerable<RequestModel.QueryStringParameter> parameters)
        {
            if (!parameters.Any())
            {
                return null;
            }

            var result = new TabObject();
            foreach (var parameter in parameters)
            {
                result.AddRow().Key(parameter.Key ?? "null").Value(parameter.Value);
            }

            return result;
        }
    }
}