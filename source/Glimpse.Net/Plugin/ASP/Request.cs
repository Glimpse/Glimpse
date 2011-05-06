using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Net.Extensibility;
using Glimpse.Net.Extensions;

namespace Glimpse.Net.Plugin.Asp
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

            var cs = request.Cookies;
            for (var i = 0; i < cs.Count; i++)
                {
                    var value = cs[i].Value;
                    var secure = cs[i].Secure.ToString();
                    var path = cs[i].Path;
                    var name = cs[i].Name;
                cookies.Add(new[] { name, path, secure, value });
            }


            var form = request.Form.Flatten();
            var querystring = request.QueryString.Flatten();

            if (form == null && querystring == null && cookies.Count <= 1) return null;
            
            return new
                       {
                           Cookies = cookies,
                           Form = form,
                           QueryString = querystring,
                       };
        }

        public void SetupInit(HttpApplication application)
        {
            throw new NotImplementedException();
        }
    }
}