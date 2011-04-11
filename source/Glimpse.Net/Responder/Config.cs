using System.ComponentModel.Composition;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class Config : GlimpseResponder
    {
        [ImportingConstructor]
        public Config(JavaScriptSerializer jsSerializer) : base(jsSerializer)
        {
        }

        public override string ResourceName
        {
            get { return "Config"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration configuration)
        {
            var response = application.Response;
            var mode = application.GetGlimpseMode();

            response.Write(
                string.Format(
                    "<html><head><title>Glimpse Config</title><script>function toggleCookie(){{var mode = document.getElementById('glimpseState'); if (mode.innerHTML==='On'){{mode.innerHTML='Off';document.cookie='glimpseState=Off; path=/;'}}else{{mode.innerHTML='On';document.cookie='glimpseState=On; path=/;'}}}}</script><head><body><h1>Glimpse Config Settings:</h1><ul><li>On = {0}</li><li>Allowed IP's = <ol>",
                    configuration.On));
            foreach (IpAddress ipAddress in configuration.IpAddresses)
            {
                response.Write(string.Format("<li>{0}</li>", ipAddress.Address));
            }
            response.Write("</ol></li><li>Allowed ContentType's = <ol>");
            foreach (ContentType contentType in configuration.ContentTypes)
            {
                response.Write(string.Format("<li>{0}</li>", contentType.Content));
            }
            response.Write(
                string.Format(
                    "</ol></li></ul><h1>Your Settings:</h1><ol><li>IP = {0}</li><li>glimpseState = <input type='checkbox' id='gChk' onclick='toggleCookie();'{2}/> <label for='gChk' id='glimpseState'>{1}</lable></li></ol>",
                    application.Request.ServerVariables["REMOTE_ADDR"], mode,
                    mode == GlimpseMode.On ? " checked" : ""));

            response.Write("<h1>Bookmarklets:</h1><p>Drag the following bookmarklets to your favorites bar for quick and easy access to Glimpse.</p><ul><li><a href=\"javascript:(function(){document.cookie='glimpseState=On; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Glimpse</a> - Used to turn Glimpse on.</li><li><a href=\"javascript:(function(){document.cookie='glimpseClientName='+ prompt('Client Name?') +'; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Glimpse Name</a> - Used to set the client name in Glimpse requests.</li></ul></body></html>");

            application.CompleteRequest();
        }
    }
}