using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            
            //Option 1
            root.AddRow().Key("Client Side").Value(request.ClientSide);
            root.AddRow().Key("Server Side").Value(request.ServerSide);

            //Option 2
            //root.AddRow().Key("______________________________________________________________________________________________ Client Side ______________________________________________________________________________________________").Value("");
            //BuildClientSide(request.ClientSide, root);
            //root.AddRow().Key("______________________________________________________________________________________________ Server Side ______________________________________________________________________________________________").Value("");
            //BuildServerSide(request.ServerSide, root);

            return root.Build();
        }

        private void BuildClientSide(ClientSide clientSide, TabObject root)
        {
            root.AddRow().Key("Cookies").Value(BuildCookies(clientSide.Cookies));
            root.AddRow().Key("Files").Value(BuildFiles(clientSide.Files));
            root.AddRow().Key("Form Variables").Value(BuildFormVariables(clientSide.FormVariables));
            root.AddRow().Key("Header Fields").Value(BuildHeaderFields(clientSide.HeaderFields));
            root.AddRow().Key("Query String").Value(BuildQueryString(clientSide.QueryString));
            root.AddRow().Key("Url").Value(clientSide.Url.ToString());
            root.AddRow().Key("Url Referrer").Value(clientSide.UrlReferrer.ToStringOrDefault());
            root.AddRow().Key("Raw Url").Value(clientSide.RawUrl);
            root.AddRow().Key("Url").Value(clientSide.Url);
            root.AddRow().Key("Url Referrer").Value(clientSide.UrlReferrer);
            root.AddRow().Key("User Agent").Value(clientSide.UserAgent);
            root.AddRow().Key("User Host Address").Value(clientSide.UserHostAddress);
            root.AddRow().Key("User Host Name").Value(clientSide.UserHostName);
        }

        private void BuildServerSide(ServerSide serverSide, TabObject root)
        {
            root.AddRow().Key("App Relative Current Execution File Path").Value(serverSide.AppRelativeCurrentExecutionFilePath);
            root.AddRow().Key("Application Path").Value(serverSide.ApplicationPath);
            root.AddRow().Key("Current Execution File Path").Value(serverSide.CurrentExecutionFilePath);
            root.AddRow().Key("Current UI Culture").Value(serverSide.CurrentUiCulture);
            root.AddRow().Key("File Path").Value(serverSide.FilePath);
            root.AddRow().Key("Path").Value(serverSide.Path);
            root.AddRow().Key("Path Info").Value(serverSide.PathInfo);
            root.AddRow().Key("Physical Application Path").Value(serverSide.PhysicalApplicationPath);
            root.AddRow().Key("Physical Path").Value(serverSide.PhysicalPath);
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

        private object BuildFiles(IEnumerable<HttpPostedFile> files)
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
            if (!headerFields.Any())
            {
                return null;
            }
            return headerFields.Where(h => h.Key.ToLower() != "cookie");
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