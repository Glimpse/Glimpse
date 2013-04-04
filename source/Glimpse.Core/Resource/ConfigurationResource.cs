using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse configuration page, which is usually where a user turns Glimpse on and off.
    /// </summary>
    public class ConfigurationResource : IPrivilegedResource, IKey
    {
        internal const string InternalName = "glimpse_config";

        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// Resource name's should be unique across a given web application. If multiple <see cref="IResource" /> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.
        /// </remarks>
        public string Name
        {
            get { return InternalName; }
        }

        /// <summary>
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return Enumerable.Empty<ResourceParameterMetadata>(); }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key. Only valid JavaScript identifiers should be used for future compatibility.
        /// </value>
        public string Key
        {
            get { return Name; }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Throws a <see cref="NotSupportedException"/> since this is a <see cref="IPrivilegedResource"/>.</exception>
        public IResourceResult Execute(IResourceContext context)
        {
            throw new NotSupportedException(string.Format(Resources.RrivilegedResourceExecuteNotSupported, GetType().Name));
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// A <see cref="IResourceResult" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if either <paramref name="context"/> or <paramref name="configuration"/> are <c>null</c>.</exception>
        /// <remarks>
        /// Use of <see cref="IPrivilegedResource" /> is reserved.
        /// </remarks>
        public IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration)
        {
            var content = new StringBuilder();
            content.Append("<!DOCTYPE html><html><head><link rel=\"shortcut icon\" href=\"http://getglimpse.com/favicon.ico\" />");
            content.Append("<style>body { margin: 0px; text-align:center; font-family:\"avante garde\", \"Century Gothic\", Serif; font-size:0.8em; line-height:1.4em; } .important { font-size:1.4em; } .content { position:absolute; left:50%; margin-left:-450px; text-align:left; width:900px; } h1, h2, h3, h4 { line-height:1.2em; font-weight:normal; } h1 { font-size:4em; } h2 { font-size:2.5em; } h3 { font-size:2em; } .logo { font-family: \"TitilliumMaps\", helvetica, sans-serif; margin:0 0 40px; position:relative; background: url(http://getglimpse.com/content/glimpseLogo.png?version=" + GlimpseRuntime.Version + ") -10px -15px no-repeat; padding: 0 0 0 140px; } .logo h1 { color:transparent; } .logo div { font-size:1.5em; margin: 25px 0 0 -10px; } .logo blockquote { width:350px; position:absolute; right:0; top:10px; } blockquote { font: 1.2em/1.6em \"avante garde\", \"Century Gothic\", Serif; width: 400px; background: url(http://getglimpse.com/Content/close-quote.gif?version=" + GlimpseRuntime.Version + ") no-repeat right bottom; padding-left: 18px; text-indent: -18px; } .footer { text-align:center; margin-bottom:30px; } blockquote:first-letter { background: url(http://getglimpse.com/Content/open-quote.gif?version=" + GlimpseRuntime.Version + ") no-repeat left top; padding-left: 18px; font: italic 1.4em \"avante garde\", \"Century Gothic\", Serif; } .myButton{width:175px; line-height: 1.2em; margin:0.25em 0; text-align:center; -moz-box-shadow:inset 0 1px 0 0 #fff;-webkit-box-shadow:inset 0 1px 0 0 #fff;box-shadow:inset 0 1px 0 0 #fff;background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#ededed),color-stop(1,#dfdfdf));background:-moz-linear-gradient(center top,#ededed 5%,#dfdfdf 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed',endColorstr='#dfdfdf');background-color:#ededed;-moz-border-radius:6px;-webkit-border-radius:6px;border-radius:6px;border:1px solid #dcdcdc;display:inline-block;color:#777;font-family:arial;font-size:24px;padding:10px 41px;text-decoration:none;text-shadow:1px 1px 0 #fff}.myButton:hover{background:-webkit-gradient(linear,left top,left bottom,color-stop(0.05,#dfdfdf),color-stop(1,#ededed));background:-moz-linear-gradient(center top,#dfdfdf 5%,#ededed 100%);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf',endColorstr='#ededed');background-color:#dfdfdf}.myButton:active{position:relative;top:1px}</style>");
            content.Append("<title>Glimpse Config</title>");
            content.Append("<head><body>");
            content.Append("<script type=\"text/javascript\"> var getCookie = function(name) { var re = new RegExp(name + \"=([^;]+)\"); var value = re.exec(document.cookie); return (value != null) ? unescape(value[1]) : null; }; if (getCookie('glimpsePolicy') == 'On') { document.write(\"<div style='background-color: #B5CDA4; border-bottom: thin solid #486E25; color: #486E25; padding: 6px; font-size: 1.2em; position: fixed; width: 100%; z-index: 499;'><strong>Glimpse cookie is SET</strong> - When you go back to your site, depending on your policies, you should see Glimpse at the bottom right of the page.</div>\"); }</script>");
            content.Append("<div class=\"content\"><div class=\"logo\"><blockquote>What Firebug is for the client, Glimpse does for the server... in other words, a client side Glimpse into what's going on in your server.</blockquote><h1>Glimpse</h1><div>A client side Glimpse to your server</div></div>");
            content.Append("<table width=\"100%\"><tr align=\"center\"><td width=\"33%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpsePolicy=On; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse On</a></td><td width=\"34%\"><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpsePolicy=; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Turn Glimpse Off</a></td><td><a class=\"myButton\" href=\"javascript:(function(){document.cookie='glimpseId='+ prompt('Client Name?') +'; path=/; expires=Sat, 01 Jan 2050 12:00:00 GMT;'; window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table>");
            content.Append("<p style=\"text-align:center\">Drag the above button to your favorites bar for quick and easy access to Glimpse.</p>");
            content.Append("<h2>Configuration Details:</h2><p>This section details the Glimpse thinks its is configured.</p><ul><li><strong>Tabs</strong>:<ul>");

            foreach (var tab in configuration.Tabs)
            {
                content.AppendFormat("<li><strong>{0}</strong> - {1} - {2}</li>", tab.Name, tab.GetType().FullName, tab.ExecuteOn);
            }

            content.Append("</ul></li></ul>");
            content.Append("<a id=\"more_link\" href=\"javascript:document.getElementById('more_link').style.display = 'none'; document.getElementById('more').style.display = 'block'\">Mode details?</a><ul id=\"more\" style=\"display:none\"><li><strong>Inspectors</strong>: <ul>");

            foreach (var inspector in configuration.Inspectors)
            {
                content.AppendFormat("<li>{0}</li>", inspector.GetType().FullName);
            }

            content.Append("</ul></li><li><strong>Runtime Policies</strong>: <ul>");

            foreach (var policy in configuration.RuntimePolicies)
            {
                content.AppendFormat("<li>{0} - {1}</li>", policy.GetType().FullName, policy.ExecuteOn);
            }

            content.Append("</ul></li><li><strong>Resources</strong>: <ul>");

            foreach (var resource in configuration.Resources)
            {
                var paramaters = string.Empty;
                if (resource.Parameters != null)
                {
                    paramaters = string.Join(", ", resource.Parameters.Select(parameter => string.Format("{0} ({1})", parameter.Name, parameter.IsRequired)).ToArray());
                }

                content.AppendFormat("<li>{0} - {1} - {2}</li>", resource.GetType().FullName, resource.Name, paramaters);
            }

            content.Append("</ul></li><li><strong>Client Scripts</strong>: <ul>");

            foreach (var scripts in configuration.ClientScripts)
            {
                content.AppendFormat("<li>{0} - {1}</li>", scripts.GetType().FullName, scripts.Order);
            }

            content.AppendFormat("</ul></li><li><strong>Framework Provider</strong>: {0}</li>", configuration.FrameworkProvider.GetType().FullName);
            content.AppendFormat("<li><strong>Html Encoder</strong>: {0}</li>", configuration.HtmlEncoder.GetType().FullName);
            content.AppendFormat("<li><strong>Logger</strong>: {0}</li>", configuration.Logger.GetType().FullName);
            content.AppendFormat("<li><strong>Persistence Store</strong>: {0}</li>", configuration.PersistenceStore.GetType().FullName);
            content.AppendFormat("<li><strong>Resource Endpoint</strong>: {0}</li>", configuration.ResourceEndpoint.GetType().FullName);
            content.AppendFormat("<li><strong>Serializer</strong>: {0}</li>", configuration.Serializer.GetType().FullName);
            content.AppendFormat("<li><strong>Default Resource</strong>: {0} - {1}</li>", configuration.DefaultResource.GetType().FullName, configuration.DefaultResource.Name);
            content.AppendFormat("<li><strong>Default Runtime Policy</strong>: {0}</li>", configuration.DefaultRuntimePolicy.GetType().FullName);
            content.AppendFormat("<li><strong>Proxy Factory</strong>: {0}</li>", configuration.ProxyFactory.GetType().FullName);
            content.AppendFormat("<li><strong>Message Broker</strong>: {0}</li>", configuration.MessageBroker.GetType().FullName);
            content.AppendFormat("<li><strong>Endpoint Base Uri</strong>: {0}</li></ul>", configuration.EndpointBaseUri); 

            content.Append("<h2>More Info:</h2>");
            content.Append("<div class=\"footer\"><span class=\"important\">For more info see <a href=\"http://getGlimpse.com\" />getGlimpse.com</a></span><br /><br /><img src=\"http://getglimpse.com/content/uservoice-icon.png\" width=\"16\" /> Have a <em>feature</em> request? <a href=\"http://getglimpse.uservoice.com\">Submit the idea</a>. &nbsp; &nbsp; <img src=\"http://getglimpse.com/content/github.gif\" /> Found an <em>error</em>? <a href=\"https://github.com/glimpse/glimpse/issues\">Help us improve</a>. &nbsp; &nbsp;<img src=\"http://getglimpse.com/content/twitter.png\" /> Have a <em>question</em>? <a href=\"http://twitter.com/#search?q=%23glimpse\">Tweet us using #glimpse</a>.</div>");
            content.Append("</body></html>");
             
            return new HtmlResourceResult(content.ToString());
        } 
    }
}