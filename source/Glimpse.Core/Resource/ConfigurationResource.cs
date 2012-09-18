using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class ConfigurationResource:IResource
    {
        internal const string InternalName = "glimpse_config";

        public string Name
        {
            get { return InternalName; }
        }

        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return Enumerable.Empty<ResourceParameterMetadata>(); }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            var content = new StringBuilder();
            content.Append("<!DOCTYPE html><html><head><link rel=\"shortcut icon\" href=\"http://getglimpse.com/favicon.ico\" />");
            content.Append("<style>body { margin: 0px; text-align:center; font-family:\"avante garde\", \"Century Gothic\", Serif; font-size:0.8em; line-height:1.4em; } .important { font-size:1.4em; } .content { position:absolute; left:50%; margin-left:-450px; text-align:left; width:900px; } h1, h2, h3, h4 { line-height:1.2em; font-weight:normal; } h1 { font-size:4em; } h2 { font-size:2.5em; } h3 { font-size:2em; } .logo { font-family: \"TitilliumMaps\", helvetica, sans-serif; margin:0 0 40px; position:relative; background: url(http://getglimpse.com/content/glimpseLogo.png) -10px -15px no-repeat; padding: 0 0 0 140px; } .logo h1 { color:transparent; } .logo div { font-size:1.5em; margin: 25px 0 0 -10px; } .logo blockquote { width:350px; position:absolute; right:0; top:10px; } blockquote { font: 1.2em/1.6em \"avante garde\", \"Century Gothic\", Serif; width: 400px; background: url(http://getglimpse.com/Content/close-quote.gif) no-repeat right bottom; padding-left: 18px; text-indent: -18px; } .footer { text-align:center; margin-bottom:30px; } blockquote:first-letter { background: url(http://getglimpse.com/Content/open-quote.gif) no-repeat left top; padding-left: 18px; font: italic 1.4em \"avante garde\", \"Century Gothic\", Serif; } .myButton{width:175px; line-height: 1.2em; margin:0.25em 0; text-align:center; -moz-box-shadow:inset 0 1px 0 0 #fff;-webkit-box-shadow:inset 0 1px 0 0 #fff;box-shadow:inset 0 1px 0 0 #fff;background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#ededed),color-stop(1,#dfdfdf));background:-moz-linear-gradient(center top,#ededed 5%,#dfdfdf 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed',endColorstr='#dfdfdf');background-color:#ededed;-moz-border-radius:6px;-webkit-border-radius:6px;border-radius:6px;border:1px solid #dcdcdc;display:inline-block;color:#777;font-family:arial;font-size:24px;padding:10px 41px;text-decoration:none;text-shadow:1px 1px 0 #fff}.myButton:hover{background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#dfdfdf),color-stop(1,#ededed));background:-moz-linear-gradient(center top,#dfdfdf 5%,#ededed 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf',endColorstr='#ededed');background-color:#dfdfdf}.myButton:active{position:relative;top:1px}</style>");
            content.Append("<title>Glimpse Config</title>");
            content.Append("<script>function toggleCookie(){var mode = document.getElementById('glimpseState'); if (mode.innerHTML==='On'){mode.innerHTML='Off';document.cookie='glimpsePolicy=Off; path=/;';}else{mode.innerHTML='On';document.cookie='glimpsePolicy=On; path=/;';}}</script>");
            content.Append("<head><body>");
            content.Append("<div class=\"content\"><div class=\"logo\"><blockquote>What Firebug is for the client, Glimpse does for the server... in other words, a client side Glimpse into what's going on in your server.</blockquote><h1>Glimpse</h1><div>A client side Glimpse to your server</div></div>");
            content.Append("<table width=\"100%\"><tr align=\"center\"><td width=\"33%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpsePolicy=On; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse On</a></td><td width=\"34%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpsePolicy=; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse Off</a></td><td><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseClientName='+ prompt('Client Name?') +'; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table>");
            content.Append("<p style=\"text-align:center\">Drag the above button to your favorites bar for quick and easy access to Glimpse.</p>");
            content.Append("<h2>More Info:</h2>");
            content.Append("<div class=\"footer\"><span class=\"important\">For more info see <a href=\"http://getGlimpse.com\" />getGlimpse.com</a></span><br /><br /><img src=\"http://getglimpse.com/content/uservoice-icon.png\" width=\"16\" /> Have a <em>feature</em> request? <a href=\"http://getglimpse.uservoice.com\">Submit the idea</a>. &nbsp; &nbsp; <img src=\"http://getglimpse.com/content/github.gif\" /> Found an <em>error</em>? <a href=\"https://github.com/glimpse/glimpse/issues\">Help us improve</a>. &nbsp; &nbsp;<img src=\"http://getglimpse.com/content/twitter.png\" /> Have a <em>question</em>? <a href=\"http://twitter.com/#search?q=%23glimpse\">Tweet us using #glimpse</a>.</div>");
            content.Append("</body></html>");
             
            return new HtmlResourceResult(content.ToString());
        }
    }
}