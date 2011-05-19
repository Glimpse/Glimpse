using System.Collections.Generic;
using System.Web;
using Glimpse.WebForms.Extensibility;
using Glimpse.WebForms.Extensions;

namespace Glimpse.WebForms.Plugin
{
    [GlimpsePlugin]
    internal class Request : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Request"; }
        }

        public object GetData(HttpApplication application)
        {
            var request = application.Request;

            var cookies = new List<object[]>
                              {
                                  new[]{"Name", "Path", "Secure", "Value"}
                              };

            var requestCookies = request.Cookies;

            foreach (var key in request.Cookies.AllKeys)
            {
                var cookie = requestCookies[key];

                if (cookie != null)
                    cookies.Add(new[] { cookie.Name, cookie.Path, cookie.Secure.ToString(), application.Server.UrlDecode(cookie.Value) });
            }

            var form = request.Form.Flatten();
            var querystring = request.QueryString.Flatten();

            //make sure there is request data
            if (form == null && querystring == null && cookies.Count <= 1) return null;
            
            return new
                       {
                           Cookies = cookies,
                           Form = form,
                           QueryString = querystring,
                           request.ApplicationPath,
                           request.AppRelativeCurrentExecutionFilePath,
                           request.CurrentExecutionFilePath,
                           request.CurrentExecutionFilePathExtension,
                           request.FilePath,
                           request.Path,
                           request.PathInfo,
                           request.PhysicalApplicationPath,
                           request.PhysicalPath,
                           request.RawUrl,
                           Url = request.Url.ToString(),
                           UrlReferrer = request.UrlReferrer == null ? null : request.UrlReferrer.ToString(),
                           request.UserAgent,
                           request.UserHostAddress,
                           request.UserHostName,
                       };
        }

        public void SetupInit(HttpApplication application)
        {
        }
    }
}