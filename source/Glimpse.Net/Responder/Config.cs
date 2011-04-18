using System.ComponentModel.Composition;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;
using Glimpse.Net.Extentions;

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

            response.Write("<!DOCTYPE html><html><head><style>body { text-align:center; font-family:\"avante garde\", \"Century Gothic\", Serif; font-size:0.8em; line-height:1.4em; } .content { position:absolute; left:50%; margin-left:-450px; text-align:left; width:900px; } h1, h2, h3, h4 { line-height:1.2em; font-weight:normal; } h1 { font-size:4em; } h2 { font-size:2.5em; } h3 { font-size:2em; } .logo { font-family: \"TitilliumMaps\", helvetica, sans-serif; margin:100px 0 40px; position:relative; background: url(http://getglimpse.com/Content/glimpseLogo.png) -10px -15px no-repeat; padding: 0 0 0 140px; } .logo h1 { color:transparent; } .logo div { font-size:1.5em; margin: 25px 0 0 -10px; } .logo blockquote { width:350px; position:absolute; right:0; top:10px; } blockquote { font: 1.2em/1.6em \"avante garde\", \"Century Gothic\", Serif; width: 400px; background: url(http://getglimpse.com/Content/close-quote.gif) no-repeat right bottom; padding-left: 18px; text-indent: -18px; } blockquote:first-letter { background: url(http://getglimpse.com/Content/open-quote.gif) no-repeat left top; padding-left: 18px; font: italic 1.4em \"avante garde\", \"Century Gothic\", Serif; } .myButton{width:175px; line-height: 1.2em; margin:0.25em 0; text-align:center; -moz-box-shadow:inset 0 1px 0 0 #fff;-webkit-box-shadow:inset 0 1px 0 0 #fff;box-shadow:inset 0 1px 0 0 #fff;background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#ededed),color-stop(1,#dfdfdf));background:-moz-linear-gradient(center top,#ededed 5%,#dfdfdf 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed',endColorstr='#dfdfdf');background-color:#ededed;-moz-border-radius:6px;-webkit-border-radius:6px;border-radius:6px;border:1px solid #dcdcdc;display:inline-block;color:#777;font-family:arial;font-size:24px;padding:10px 41px;text-decoration:none;text-shadow:1px 1px 0 #fff}.myButton:hover{background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#dfdfdf),color-stop(1,#ededed));background:-moz-linear-gradient(center top,#dfdfdf 5%,#ededed 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf',endColorstr='#ededed');background-color:#dfdfdf}.myButton:active{position:relative;top:1px}</style><title>Glimpse Config</title><script>function toggleCookie(){var mode = document.getElementById('glimpseState'); if (mode.innerHTML==='On'){mode.innerHTML='Off';document.cookie='glimpseState=Off; path=/;';}else{mode.innerHTML='On';document.cookie='glimpseState=On; path=/;';}}</script><head><body><div class='content'><div class=\"logo\"><blockquote>What Firebug is for the client, Glimpse does for the server... in other words, a client side Glimpse into whats going on in your server.</blockquote><h1>Glimpse</h1><div>A client side Glimpse to your server</div></div><table width=\"100%\"><tr align=\"center\"><td width=\"50%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseState=On; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse On</a></td><td><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseClientName='+ prompt('Client Name?') +'; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table><p style=\"text-align:center\">Drag the above button to your favorites bar for quick and easy access to Glimpse.</p>");
            response.Write(string.Format("<h2>Glimpse Config Settings:</h2><p>This section details the Glimpse settings in your web.config file.</p><ul><li>On = {0}</li><li>Allowed IP's = <ol>", configuration.On));

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
                    "</ol></li></ul><h2>Your Settings:</h2><p>This section tells you how Glimpse sees your requests.</p><ul><li>IP = {0}</li><li>glimpseState = <label for='gChk' id='glimpseState'>{1}</label></li></ul>",
                    application.Request.ServerVariables["REMOTE_ADDR"], mode,
                    mode == GlimpseMode.On ? " checked" : ""));

            response.Write("<h2>More Info:</h2><p><em>For more info see <a href='http://getGlimpse.com'/>getGlimpse.com</a> or follow <a href='http://twitter.com/#!/search/%23glimpse'/>#glimpse</a> on Twitter</em></p></body></html>");

            application.CompleteRequest();
        }
    }
}