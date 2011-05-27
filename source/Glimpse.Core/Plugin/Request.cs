using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Microsoft.Web.Infrastructure.DynamicValidationHelper;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin]
    internal class Request : IGlimpsePlugin, IProvideGlimpseHelp
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

            //To avoid validation exceptions, input values are read like this
            Func<NameValueCollection> formData;
            Func<NameValueCollection> queryStringData;
            ValidationUtility.GetUnvalidatedCollections(application.Context, out formData, out queryStringData);

            var form = formData().Flatten();
            var querystring = queryStringData().Flatten();

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

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Request"; }
        }
    }
}