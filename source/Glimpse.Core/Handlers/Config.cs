using System.ComponentModel.Composition;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class Config : IGlimpseHandler
    {
        public string ResourceName
        {
            get { return "Config"; }
        }

        private GlimpseConfiguration Configuration { get; set; }

        [ImportingConstructor]
        public Config(GlimpseConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ProcessRequest(HttpContextBase context)
        {
            var response = context.Response;
            var mode = context.GetGlimpseMode();

            response.Write("<!DOCTYPE html><html><head><link rel=\"shortcut icon\" href=\"http://getglimpse.com/favicon.ico\" />");
            response.Write("<style>body { margin: 0px; text-align:center; font-family:\"avante garde\", \"Century Gothic\", Serif; font-size:0.8em; line-height:1.4em; } .important { font-size:1.4em; } .content { position:absolute; left:50%; margin-left:-450px; text-align:left; width:900px; } h1, h2, h3, h4 { line-height:1.2em; font-weight:normal; } h1 { font-size:4em; } h2 { font-size:2.5em; } h3 { font-size:2em; } .logo { font-family: \"TitilliumMaps\", helvetica, sans-serif; margin:0 0 40px; position:relative; background: url("+ context.GlimpseResourcePath("logo.png") +") -10px -15px no-repeat; padding: 0 0 0 140px; } .logo h1 { color:transparent; } .logo div { font-size:1.5em; margin: 25px 0 0 -10px; } .logo blockquote { width:350px; position:absolute; right:0; top:10px; } blockquote { font: 1.2em/1.6em \"avante garde\", \"Century Gothic\", Serif; width: 400px; background: url(http://getglimpse.com/Content/close-quote.gif) no-repeat right bottom; padding-left: 18px; text-indent: -18px; } .footer { text-align:center; margin-bottom:30px; } blockquote:first-letter { background: url(http://getglimpse.com/Content/open-quote.gif) no-repeat left top; padding-left: 18px; font: italic 1.4em \"avante garde\", \"Century Gothic\", Serif; } .myButton{width:175px; line-height: 1.2em; margin:0.25em 0; text-align:center; -moz-box-shadow:inset 0 1px 0 0 #fff;-webkit-box-shadow:inset 0 1px 0 0 #fff;box-shadow:inset 0 1px 0 0 #fff;background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#ededed),color-stop(1,#dfdfdf));background:-moz-linear-gradient(center top,#ededed 5%,#dfdfdf 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed',endColorstr='#dfdfdf');background-color:#ededed;-moz-border-radius:6px;-webkit-border-radius:6px;border-radius:6px;border:1px solid #dcdcdc;display:inline-block;color:#777;font-family:arial;font-size:24px;padding:10px 41px;text-decoration:none;text-shadow:1px 1px 0 #fff}.myButton:hover{background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#dfdfdf),color-stop(1,#ededed));background:-moz-linear-gradient(center top,#dfdfdf 5%,#ededed 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf',endColorstr='#ededed');background-color:#dfdfdf}.myButton:active{position:relative;top:1px}</style>");
            response.Write("<title>Glimpse Config</title>");
            response.Write("<script>function toggleCookie(){var mode = document.getElementById('glimpseState'); if (mode.innerHTML==='On'){mode.innerHTML='Off';document.cookie='glimpseState=Off; path=/;';}else{mode.innerHTML='On';document.cookie='glimpseState=On; path=/;';}}</script>");
            response.Write("<head><body>");

            //temporary measure to help users move from glimpse/config to glimpse.asx
            if (context.Request.QueryString["redirect"]=="1")
            {
                response.Write("<div style='background-color: lightCoral; border: thin solid maroon; color: maroon; padding: 6px; font-size: 1.2em; position: fixed; width: 100%; z-index: 500;'><strong>Redirected!</strong> - Glimpse will no longer support the 'Glimpse/Config' url in the next version. Please use 'Glimpse.axd' instead.</div>");
            }
            if (context.Request.Cookies["glimpseState"] != null && context.Request.Cookies["glimpseState"].Value == "On")
            {
                response.Write("<div style='background-color: #B5CDA4; border-bottom: thin solid #486E25; color: #486E25; padding: 6px; font-size: 1.2em; position: fixed; width: 100%; z-index: 499;'><strong>Glimpse is now ON</strong> - When you go back to your site you should see Glimpse at the bottom right of the page.</div>");
            }

            response.Write("<div class=\"content\"><div class=\"logo\"><blockquote>What Firebug is for the client, Glimpse does for the server... in other words, a client side Glimpse into whats going on in your server.</blockquote><h1>Glimpse</h1><div>A client side Glimpse to your server</div></div>");
            response.Write("<table width=\"100%\"><tr align=\"center\"><td width=\"33%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseState=On; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse On</a></td><td width=\"34%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseState=; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse Off</a></td><td><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseClientName='+ prompt('Client Name?') +'; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table>");
            response.Write("<p style=\"text-align:center\">Drag the above button to your favorites bar for quick and easy access to Glimpse.</p>");
            response.Write(string.Format("<h2>Glimpse Config Settings:</h2><p>This section details the Glimpse settings in your web.config file.</p><ul><li>On = {0}</li><li>Allowed IP's = <ol>", Configuration.Enabled));

            foreach (IpAddress ipAddress in Configuration.IpAddresses)
            {
                response.Write(string.Format("<li>{0}</li>", ipAddress.Address));
            }
            response.Write("</ol></li><li>Allowed ContentType's = <ol>");
            foreach (ContentType contentType in Configuration.ContentTypes)
            {
                response.Write(string.Format("<li>{0}</li>", contentType.Content));
            }
            response.Write("</ol></li><li>Blacklisted Plugins = <ol>");
            foreach (var typeName in Configuration.PluginBlacklist.TypeNames())
            {
                response.Write("<li>" + typeName + "</li>");
            }

            response.Write(string.Format("</ol></ul><h2>Your Settings:</h2><p>This section tells you how Glimpse sees your requests.</p><ul><li>IP = {0}</li><li>glimpseState = <label for='gChk' id='glimpseState'>{1}</label></li></ul>", context.Request.UserHostAddress, mode));

            response.Write("<h2>Plugins:</h2><p>This is the list of Glimpse plugins for this web application. Glimpse plugins show up as individual tabs in the Glimpse client. Plugins with a strike line through them are not loaded. You can stop a plugin from being loaded by adding it to the glimpse/pluginBlacklist section of your web.config.</p><ul>");
            foreach (var plugin in Module.Plugins)
            {
                response.Write("<li>" + plugin.Value.GetType() + "</li>");
            }

            foreach (var typeName in Configuration.PluginBlacklist.TypeNames())
            {
                response.Write("<li><s>" + typeName + "</s></li>");
            }

            response.Write("</ul>");

            response.Write("<h2>More Info:</h2>");
            response.Write("<div class=\"footer\"><span class=\"important\">For more info see glimpse.readme.txt in your project's App_Readme directory or at <a href=\"http://getGlimpse.com\" />getGlimpse.com</a></span><br /><br /><img src=\"http://getglimpse.com/content/uservoice-icon.png\" width=\"16\" /> Have a <em>feature</em> request? <a href=\"http://getglimpse.uservoice.com\">Submit the idea</a>. &nbsp; &nbsp; <img src=\"http://getglimpse.com/content/github.gif\" /> Found an <em>error</em>? <a href=\"https://github.com/glimpse/glimpse/issues\">Help us improve</a>. &nbsp; &nbsp;<img src=\"http://getglimpse.com/content/twitter.png\" /> Have a <em>question</em>? <a href=\"http://twitter.com/#search?q=%23glimpse\">Tweet us using #glimpse</a>.</div>");

            response.Write("</body></html>");
        }
    }
}