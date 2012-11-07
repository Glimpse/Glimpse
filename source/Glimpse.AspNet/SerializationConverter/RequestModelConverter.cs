using System.Collections.Generic;
using Glimpse.AspNet.Extensions;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class RequestModelConverter : SerializationConverter<RequestModel>
    {
        public override object Convert(RequestModel request)
        {
            var root = new TabObject();
            root.AddRow().Key("Cookies").Value(request.Cookies.ToTable()); 
            root.AddRow().Key("Query String").Value(request.QueryString.ToTable());
            root.AddRow().Key("Url").Value(request.Url.ToString());
            root.AddRow().Key("Url Referrer").Value(request.UrlReferrer.OrNull());
            root.AddRow().Key("App Relative Current Execution File Path").Value(request.AppRelativeCurrentExecutionFilePath);
            root.AddRow().Key("Application Path").Value(request.ApplicationPath);
            root.AddRow().Key("Current Execution File Path").Value(request.CurrentExecutionFilePath);
            root.AddRow().Key("Current UI Culture").Value(request.CurrentUiCulture);
            root.AddRow().Key("File Path").Value(request.FilePath);
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
    }
}