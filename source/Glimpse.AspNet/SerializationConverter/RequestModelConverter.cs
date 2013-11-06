using System.Collections.Generic;
using System.Linq;
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

            root.AddRow().Key("Browser").Value(request.Browser);
            root.AddRow().Key("Cookies").Value(BuildCookies(request.Cookies));
            root.AddRow().Key("Current UI Culture").Value(request.CurrentUiCulture);
            root.AddRow().Key("Files").Value(BuildFiles(request.Files));
            root.AddRow().Key("Form Variables").Value(BuildFormVariables(request.FormVariables));
            root.AddRow().Key("Header Fields").Value(BuildHeaderFields(request.HeaderFields));
            root.AddRow().Key("Query String").Value(BuildQueryString(request.QueryString));
            root.AddRow().Key("Url").Value(request.Url.ToString());
            root.AddRow().Key("Url Referrer").Value(request.UrlReferrer.ToStringOrDefault());
            root.AddRow().Key("Raw Url").Value(request.RawUrl);
            root.AddRow().Key("Request Type").Value(request.RequestType);
            root.AddRow().Key("Url").Value(request.Url);
            root.AddRow().Key("Url Referrer").Value(request.UrlReferrer);
            root.AddRow().Key("User Agent").Value(request.UserAgent);
            root.AddRow().Key("User Host Address").Value(request.UserHostAddress);
            root.AddRow().Key("User Host Name").Value(request.UserHostName);

            root.AddRow().Key("Path").Value(BuildPathFields(request));

            return root.Build();
        }

        private TabObject BuildPathFields(RequestModel request)
        {
            var root = new TabObject();

            root.AddRow().Key("App Relative Current Execution File Path").Value(request.AppRelativeCurrentExecutionFilePath);
            root.AddRow().Key("Application Path").Value(request.ApplicationPath);
            root.AddRow().Key("Current Execution File Path").Value(request.CurrentExecutionFilePath);
            root.AddRow().Key("File Path").Value(request.FilePath);
            root.AddRow().Key("Path").Value(request.Path);
            root.AddRow().Key("Path Info").Value(request.PathInfo);
            root.AddRow().Key("Physical Application Path").Value(request.PhysicalApplicationPath);
            root.AddRow().Key("Physical Path").Value(request.PhysicalPath);

            return root;
        }

        public TabSection BuildCookies(IEnumerable<RequestModel.Cookie> cookies)
        {
            var cookiesList = cookies as IList<RequestModel.Cookie> ?? cookies.ToList();

            if (cookies == null || !cookiesList.Any())
            {
                return null;
            }

            var result = new TabSection("Name", "Value", "Path", "Secure");
            foreach (var cookie in cookiesList)
            {
                result.AddRow().Column(cookie.Name).Column(cookie.Value).Column(cookie.Path).Column(cookie.IsSecure);
            }

            return result;
        }

        private object BuildFiles(IEnumerable<RequestModel.PostedFile> files)
        {
            if (files == null || !files.Any())
            {
                return null;
            }
            return files;
        }

        private object BuildFormVariables(IEnumerable<RequestModel.FormVariable> formVariables)
        {
            if (formVariables == null || !formVariables.Any())
            {
                return null;
            }
            return formVariables;
        }

        private object BuildHeaderFields(IEnumerable<RequestModel.HeaderField> headerFields)
        {
            var headerFieldList = headerFields as IList<RequestModel.HeaderField> ?? headerFields.ToList();
            if (headerFields == null || !headerFieldList.Any())
            {
                return null;
            }
            return headerFieldList.Where(h => h.Key.ToLower() != "cookie");
        }

        public TabObject BuildQueryString(IEnumerable<RequestModel.QueryStringParameter> parameters)
        {
            var queryStringParametersList = parameters as IList<RequestModel.QueryStringParameter> ?? parameters.ToList();

            if (parameters == null || !queryStringParametersList.Any())
            {
                return null;
            }

            var result = new TabObject();
            foreach (var parameter in queryStringParametersList)
            {
                result.AddRow().Key(parameter.Key ?? "null").Value(parameter.Value);
            }

            return result;
        }
    }
}