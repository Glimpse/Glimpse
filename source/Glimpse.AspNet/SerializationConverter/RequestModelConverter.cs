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

            //root.AddRow().Key("Browser").Value(request.Browser);
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

        public IEnumerable<RequestModel.Cookie> BuildCookies(IEnumerable<RequestModel.Cookie> cookies)
        {
            var cookiesList = cookies as IList<RequestModel.Cookie> ?? cookies.ToList();

            if (cookies == null || !cookiesList.Any())
            {
                return null;
            }

             return cookiesList;
        }

        private IEnumerable<RequestModel.PostedFile> BuildFiles(IEnumerable<RequestModel.PostedFile> files)
        {
            var filesList = files as IList<RequestModel.PostedFile> ?? files.ToList();

            if (files == null || !filesList.Any())
            {
                return null;
            }
            return filesList;
        }

        private IEnumerable<RequestModel.FormVariable> BuildFormVariables(IEnumerable<RequestModel.FormVariable> formVariables)
        {
            var formVariablesList = formVariables as IList<RequestModel.FormVariable> ?? formVariables.ToList();
            if (!formVariablesList.Any())
            {
                return null;
            }
            return formVariablesList;
        }

        private IEnumerable<RequestModel.HeaderField> BuildHeaderFields(IEnumerable<RequestModel.HeaderField> headerFields)
        {
            var headerFieldsList = headerFields as IList<RequestModel.HeaderField> ?? headerFields.ToList();

            if (!headerFieldsList.Any())
            {
                return null;
            }
            return headerFieldsList.Where(h => h.Key.ToLower() != "cookie");
        }

        private IEnumerable<RequestModel.QueryStringParameter> BuildQueryString(IEnumerable<RequestModel.QueryStringParameter> queryStringParameters)
        {
            var queryStringParametersList = queryStringParameters as IList<RequestModel.QueryStringParameter> ?? queryStringParameters.Where(p => p.Key != null).ToList();

            if (!queryStringParametersList.Any())
            {
                return null;
            }

            return queryStringParametersList;
        }
    }
}