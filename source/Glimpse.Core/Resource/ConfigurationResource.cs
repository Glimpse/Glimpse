using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Glimpse.Core.Support;

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

            content.Append("<!DOCTYPE html><html><head><title>Glimpse - Configuration Page</title><link rel=\"shortcut icon\" href=\"http://getglimpse.com/content/_v1/app-config-favicon.png?version=" + GlimpseRuntime.Version + "\" />");
            content.Append("<style>*, *:before, *:after{-webkit-box-sizing: border-box;-moz-box-sizing: border-box;-ms-box-sizing: border-box;-o-box-sizing: border-box;box-sizing: border-box;}body{margin: 0;font-family: \"Segoe UI Web Regular\",\"Segoe UI\",\"Helvetica Neue\",Helvetica,Arial serif;font-size: 1em;line-height: 1.6em;}.code{font-size: 1.45em;font-family:monospace;}h1, h2, h3{font-weight: normal;}header{color: #fff;background-color: #323d42;height: 450px;}header table{width: 100%;}header .detail{text-align: center;width: 250px;}header h2{position: relative;}.inner{margin:0 auto;max-width: 1200px;width: 80%;min-width: 900px;vertical-align: top;padding-top: 1em;}.button{width: 250px;line-height: 1.2em;margin: 0.5em auto;text-align: center;font-size: 24px;padding: 10px 41px;text-decoration: none;display: block;color: white;border: 3px solid white;background-color: #434f54;}.button:hover{background-color: #3f4a4f;}.message{font-size: 0.5em;line-height: 1em;width: 125px;left: -150px;top: 20px;position: absolute; font-style:italic;}img{border:0px;}.center{text-align: center;}.notification{margin-top:22px; padding: 17px; font-size: 1.2em; width: 250px; text-align: center; float:right; }.notification-success{background-color: #B5CDA4; border: thin solid #486E25; color: #486E25;}.notification-fail{background-color: #E4BBB1; border: thin solid #DA6953; color: #DA6953;}.version span{font-size:0.8em;font-style:italic;}</style>");
            content.Append("<script type=\"text/javascript\">var toggleClass = function(name) { toggleItems(document.getElementsByClassName(name)); }, toggleItems = function(e) { for(var i = 0; i < e.length; i++) { toggleItem(e[i]); } }, toggleItem = function(e) { if(e.style.display == 'none') { e.style.display = ''; } else { e.style.display = 'none'; } };</script>");
            content.Append("</head><body>");
            content.Append("<header><div class=\"inner\"><table><tr><td class=\"logo\"><a href=\"http://getglimpse.com/\" title=\"Glimpse Home :D\"><img width=\"325\" src=\"http://getglimpse.com/content/_v1/app-config-logo.png?version=" + GlimpseRuntime.Version + "\" alt=\"Glimpse Home :D\"/></a></td><td class=\"detail\"><div class=\"version\">v" + GlimpseRuntime.Version + " <span>(core)</span></div><h2>Bookmarklets<div class=\"message\">“Drag us to your favorites bar for quick and easy access to Glimpse”</div></h2><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpsePolicy=On;path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Turn Glimpse On</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpsePolicy=;path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Turn Glimpse Off</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpseId='+ prompt('Client Name?') +';path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table></div></header>");
            content.Append("<div class=\"inner\">");
            content.Append("<script type=\"text/javascript\"> var getCookie = function(name) { var re = new RegExp(name + \"=([^;]+)\"); var value = re.exec(document.cookie); return (value != null) ? unescape(value[1]) : null; }; if (getCookie('glimpsePolicy') == 'On') { document.write(\"<div class='notification notification-success'><strong>Glimpse cookie is SET</strong> When you go back to your site, depending on your policies, you should see Glimpse at the bottom right of the page.</div>\"); } else { document.write(\"<div class='notification notification-fail'><strong>Glimpse cookie NOT set</strong> By default for Glimpse to run on given requests, the `glimpsePolicy` cookie needs to be set. We have made this simple by providing the above buttons :D</div>\"); }</script>");

            content.Append("<ul><li><strong>Tabs</strong>:<ul>");

            foreach (var tab in configuration.Tabs)
            {
                content.AppendFormat("<li><strong>{0}</strong> - <span class=\"code\">{1}</span><span style=\"display:none\" class=\"more-detail\"> - <em>{2}</em></span></li>", tab.Name, tab.GetType().FullName, tab.ExecuteOn);
            }

            content.Append("</ul></li><li><strong>Runtime Policies</strong>: <ul>");

            foreach (var policy in configuration.RuntimePolicies)
            {
                content.AppendFormat("<li><span class=\"code\">{0}</span><span style=\"display:none\" class=\"more-detail\"> - <em>{1}</em></span></li>", policy.GetType().FullName, policy.ExecuteOn);
            }

            content.Append("</ul></li></ul>");
            content.Append("<a class=\"more-detail\" href=\"javascript:return true;\" onclick=\"toggleClass('more-detail')\" style=\"display:block\">More details?</a><a class=\"more-detail\" href=\"javascript:return true;\" onclick=\"toggleClass('more-detail')\" style=\"display:none\">Less details?</a><div class=\"more-detail\" style=\"display:none\">");
            content.Append("<h3>Detailed Settings:</h3><ul><li><strong>Inspectors</strong>: <ul>");

            foreach (var inspector in configuration.Inspectors)
            {
                content.AppendFormat("<li><span class=\"code\">{0}</span></li>", inspector.GetType().FullName);
            }

            content.Append("</ul></li><li><strong>Resources</strong>: <ul>");

            foreach (var resource in configuration.Resources)
            {
                var paramaters = string.Empty;
                if (resource.Parameters != null)
                {
                    paramaters = string.Join(", ", resource.Parameters.Select(parameter => string.Format("{0} ({1})", parameter.Name, parameter.IsRequired)).ToArray());
                }

                content.AppendFormat("<li><strong>{0}</strong> - <span class=\"code\">{1}</span> - <em>{2}</em></li>", resource.Name, resource.GetType().FullName, paramaters);
            }

            content.Append("</ul></li><li><strong>Client Scripts</strong>: <ul>");

            foreach (var scripts in configuration.ClientScripts)
            {
                content.AppendFormat("<li><span class=\"code\">{0}</span> - {1}</li>", scripts.GetType().FullName, scripts.Order);
            }

            content.AppendFormat("</ul></li><li><strong>Framework Provider</strong>: <span class=\"code\">{0}</span></li>", configuration.FrameworkProvider.GetType().FullName);
            content.AppendFormat("<li><strong>Html Encoder</strong>: <span class=\"code\">{0}</span></li>", configuration.HtmlEncoder.GetType().FullName);
            content.AppendFormat("<li><strong>Logger</strong>: <span class=\"code\">{0}</span></li>", configuration.Logger.GetType().FullName);
            content.AppendFormat("<li><strong>Persistence Store</strong>: <span class=\"code\">{0}</span></li>", configuration.PersistenceStore.GetType().FullName);
            content.AppendFormat("<li><strong>Resource Endpoint</strong>: <span class=\"code\">{0}</span></li>", configuration.ResourceEndpoint.GetType().FullName);
            content.AppendFormat("<li><strong>Serializer</strong>: <span class=\"code\">{0}</span></li>", configuration.Serializer.GetType().FullName);
            content.AppendFormat("<li><strong>Default Resource</strong>: <span class=\"code\">{0}</span> - <em>{1}</em></li>", configuration.DefaultResource.GetType().FullName, configuration.DefaultResource.Name);
            content.AppendFormat("<li><strong>Default Runtime Policy</strong>: <span class=\"code\">{0}</span></li>", configuration.DefaultRuntimePolicy.GetType().FullName);
            content.AppendFormat("<li><strong>Proxy Factory</strong>: <span class=\"code\">{0}</span></li>", configuration.ProxyFactory.GetType().FullName);
            content.AppendFormat("<li><strong>Message Broker</strong>: <span class=\"code\">{0}</span></li>", configuration.MessageBroker.GetType().FullName);
            content.AppendFormat("<li><strong>Endpoint Base Uri</strong>: <span class=\"code\">{0}</span></li></ul>", configuration.EndpointBaseUri);
            
            content.Append("<h3>Registered Packages:</h3>");
            content.Append("<p>NOTE, doesn't represent all the glimpse dependent Nuget packages you have installed, just the ones that have registered as a Glimpse Nuget package</p>");
            content.Append("<ul>");

            var registeredPackages = NuGetPackage.GetRegisteredPackageVersions();
            foreach (var registeredPackage in registeredPackages)
            {  
                content.AppendFormat("<li>{0} - {1}</li>", registeredPackage.Key, registeredPackage.Value);
            }
        
            content.AppendFormat("</ul>");
            content.Append("</div>");
            content.Append("</div>");
            content.Append("<footer><div class=\"inner\"><p class=\"center\">For more info see <a href=\"http://getglimpse.com\">getGlimpse.com</a></p><div class=\"center\"><img src=\"http://getglimpse.com/content/github.gif\"> Found an <em>error</em>? <a href=\"https://github.com/glimpse/glimpse/issues\">Help us improve</a>.   <img src=\"http://getglimpse.com/content/twitter.png\"> Have a <em>question</em>? <a href=\"http://twitter.com/#search?q=%23glimpse\">Tweet us using #glimpse</a>.</div></div></div></footer>");
            content.Append("</body></html>");
             
            return new HtmlResourceResult(content.ToString());
        } 
    }
}