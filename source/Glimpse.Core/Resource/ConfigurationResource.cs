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
        /// 
        public IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration)
        {
            var content = new StringBuilder();
            var packages = this.FindPackages();
            var glimpseLogoDataUri = "data:image/png;base64,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";
            var githubDataUri = "data:image/gif;base64,R0lGODlhEAAQAOYAAPr6+hoaGvDw8IqKiiwsLNra2ggICKSkpKqqqujo6Pz8/NjY2CAgIAICAoaGhi4uLsDAwBISEpaWliQkJMzMzDo6OsLCwnh4eL6+vgQEBHJycry8vCYmJgYGBioqKhgYGEhISD4+PoyMjGZmZmxsbG5ubpKSktLS0lZWVjw8PFRUVLCwsKysrCgoKLa2tqamprKysmRkZISEhDIyMo6Oju7u7rS0tPj4+GJiYtbW1sjIyEBAQJSUlKKiokpKSvLy8hwcHAwMDObm5iIiIs7OzgAAAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAAAAAAALAAAAAAQABAAAAewgEaCg4ISAwCEiYMJEQ0LiooCM0NCAgs/kIMCNUY0BiyEBzshDiU6FyQUE0UeMoIHRRMeRUUQBB8WAUVAGoIVRQsCHSAKDwFGA0UvCoIoRQcbRThGxkYiRQiDBRy0DDlGBMfJK4QqHTwJRgC3RiZFIxuDPgYI6QocEQAUBkUMggW/igS5AAAGAkQYBtgQlCKDixMgihDJBK5BDwshihSgiKEFrQwOKAqCoCGGhBsUAwEAOw==";
            var twitterDataUri = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAdpJREFUeNqUU79v00AYfbYvNhcMCZXCwBKPFR1AYkFsCHUmCImx/wD8C4z8D1RdGBkzsTGwdQNVIMGAYhYU0go1UWPH95PvLonTAkL4pO90Pt973/vedxc8f/v+Vi9Nh902z9BgnBZlfnx2NmBdfmn4eKefXeesCR6TUmWvj74OWZrElNni+1w2ImAh0OE8Y1EQYFZpomg2AjdZA7aQEpU29Y9CaDw5+ODXb57dgfkHs8MSgYAwut5UxCqt5/eq7Dlt0pjfCAQRKAlxTgFvheDx0tCH+0f1/uU4wqu97QtqHZYJkiFJgV0lIjyiuPWH3CgOqRxDyZZqyTo4LKuUgqZC1YqB2xA2WSp48eAG+tc2ZNNKYp2fEYPDhm4yBHZluFBE1tviMEmMg08znFaWsgU+pNmccxiHZYImZ5RcGXmyMHi03cX+xylGhcHTdyd+P6XaXt7fwrhY+O8kCuCwoTPCiXfmuCiVxu0esLfTwdU0QZi06nA9WZ+ztYlaAb6ETSvHc42bnQi799ILRo5mBZW4csFj1NLEaVnS1YxQ0Ho9hDCY/vz79W4z5jGV9G1U+eEoz+5mffSvtP/vJc4LHObfyAOdM6PV4Mt4Mvz8Y9LoOQcWeWTt4JcAAwDEmRKkDQBEqwAAAABJRU5ErkJggg==";

            content.Append("<!DOCTYPE html><html><head><title>Glimpse - Configuration Page</title><link rel=\"shortcut icon\" href=\"http://getglimpse.com/content/_v1/app-config-favicon.png?version=" + GlimpseRuntime.Version + "\" />");
            content.Append("<style>*, *:before, *:after{-webkit-box-sizing: border-box;-moz-box-sizing: border-box;-ms-box-sizing: border-box;-o-box-sizing: border-box;box-sizing: border-box;}body{margin: 0;font-family: \"Segoe UI Web Regular\",\"Segoe UI\",\"Helvetica Neue\",Helvetica,Arial serif;font-size: 1em;line-height: 1.6em;}pre{margin:0}.code{font-size: 1.45em;font-family:monospace;}h1, h2, h3{font-weight: normal;}header{color: #fff;background-color: #323d42;height: 450px;}header table{width: 100%;}header .detail{text-align: center;width: 250px;}header h2{position: relative;margin-bottom:0;}header .find-more{color: white;font-size: 0.8em;margin-top: 0; }header .find-more:hover,header .find-more:active{color: white;}.inner{margin:0 auto;max-width: 1200px;width: 80%;min-width: 900px;vertical-align: top;padding-top: 1em;}.button{width: 250px;line-height: 1.2em;margin: 0.5em auto;text-align: center;font-size: 24px;padding: 10px 41px;text-decoration: none;display: block;color: white;border: 3px solid white;background-color: #434f54;}.button:hover{background-color: #3f4a4f;}.button-small{font-size: 0.95em;}.inner .button{border-color: #ccc;}.message{font-size: 0.5em;line-height: 1em;width: 125px;left: -150px;top: 20px;position: absolute; font-style:italic;}img{border:0px;}.center{text-align: center;}.notification{ padding: 17px; font-size: 1.2em; text-align: center; }.notification-success{background-color: #B5CDA4; color: #486E25;}.notification-fail{background-color: #E4BBB1; color: #DA6953;}.notification-warn{background-color: #E4D1A9; color: #DA9221;}.version span{font-size:0.8em;font-style:italic;}.warn{color:red; font-weight:bold;}.package-version{font-size:0.7em;}.root > li {margin-bottom:15px;}.more-help h3{margin: 40px 0 5px; text-align: center;}.side-bar{float:right;width:250px;margin-top:22px;}.out-dated{padding-bottom:1px 0 18px;text-align:center;background-color: #FFF9B0;color: goldenrod;}.code-holder{padding:10px; margin:10px 0; background-color: #eee;font-family: monospace;line-height: 1.3em;font-size: 1.3em;}.close-section {  position: absolute; right: -19px; border-radius: 20px; font-weight: bold; height: 40px; width: 40px; text-align: center; font-size: 140%; line-height: 120%; margin-top: -9px; cursor: pointer; padding:0}.code-comment{color:#52AA00;}.code-content{margin-bottom:10px}.configuration-holder{display:none;position: relative}.description{text-align:center;text-align: center;font-size: 1.2em;font-weight: normal;}.description strong{text-decoration: underline;font-size: 1.1em;}.configuration-options-item,.code-section-child{display:none;}</style>");
            content.Append("<script type=\"text/javascript\">var toggleClass = function(name) { toggleItems(document.getElementsByClassName(name)); }, toggleItems = function(e) { for(var i = 0; i < e.length; i++) { toggleItem(e[i]); } }, toggleItem = function(e) { if(e.style.display == 'none') { e.style.display = ''; } else { e.style.display = 'none'; } };</script>");
            content.Append("</head><body>");
            content.Append("<header><div class=\"inner\"><table><tr><td class=\"logo\"><a href=\"http://getglimpse.com/\" title=\"Glimpse Home :D\"><img width=\"325\" src=\"" + glimpseLogoDataUri + "\" alt=\"Glimpse Home :D\"/></a></td><td class=\"detail\"><div class=\"version\">v" + GlimpseRuntime.Version + " <span>(core)</span></div><h2>Bookmarklets<div class=\"message\">“Drag us to your favorites bar for quick and easy access to Glimpse”</div></h2><a href=\"http://getglimpse.com/Help/First-Run#Glimpse-Bookmarklets\" class=\"find-more\" target=\"new\">find out more</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpsePolicy=On;path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Turn Glimpse On</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpsePolicy=;path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Turn Glimpse Off</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpseId='+ prompt('Client Name?') +';path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table></div></header>");
            content.Append("<div class=\"inner\">");

            //Duplicate resource 
            var duplicateResources = DetectDuplicateResources(configuration.Resources);
            if (duplicateResources.Any())
            {
                var instances = string.Join(", ", duplicateResources.Select(x => string.Format("<strong>{0}</strong>", x)).ToArray());
                content.AppendFormat("<div class='notification notification-fail'><strong>Houston we have a problem :(</strong><br /> We have detected that the following resources have been duplicated - {0}. Typically when this happens, Glimpse will fail to operate correctly and lead to ambiguous references.</div>", instances);
            }

            //Update notification
            content.Append("<script type=\"text/javascript\">var getOptions = function() { var data = localStorage.getItem('glimpseOptions'); return data != null ? JSON.parse(data) : {}; }, options = getOptions(), currentHash = options.hash, checkUri = options.versionCheckUri, viewUri = options.versionViewUri, hasNewer = options.hasNewerVersion; if (hasNewer && checkUri.indexOf('hash=' + currentHash) > -1) { document.write(\"<div class='out-dated'><h3>Really sorry to have to say this but you are out of date :(</h3>A new version of Glimpse is waiting for you... it's very lonely without you. It will be worth your while updating. <br /><a href='\" + viewUri + \"' target='new'>See what you are missing out on</a></div>\"); }</script>");

            //Configuration generator
            content.Append("<div class=\"configuration-holder\"><h3>Configuration Helper</h3>");
            content.Append("<div class=\"description warn\">NOTE: The below is <strong>only designed</strong> to help generate the config which needs be copied over to your <strong>web.config</strong></div>");
            content.Append("<div class=\"close-section button button-small\">x</div>");
            content.Append("<div class=\"code-holder\">");

            content.AppendFormat("<pre class=\"code-content\">{0}</pre>", Escape("<glimpse defaultRuntimePolicy=\"On\" endpointBaseUri=\"~/Glimpse.axd\">"));

            content.AppendFormat("<pre class=\"code-content code-logging configuration-options-item\">{0}</pre>", Escape("    <logging level=\"Trace\" />"));

            content.AppendFormat("<pre class=\"code-comment code-content code-section-tabs-off\">{0}</pre>", Escape("    <!-- Tab Management\n    <tabs>\n        <ignoredTypes>\n            <add type=\"{Namespace.Type, AssemblyName}\"/>\n        </ignoredTypes>\n    </tabs>\n    -->"));

            content.AppendFormat("<div class=\"code-content code-section-tabs code-section-child\" data-altState=\"code-section-tabs-off\"><pre>{0}</pre>", Escape("    <tabs>"));
            content.Append("<div class=\"code-tabs-ignoredTypes\">");
            content.AppendFormat("<pre>{0}</pre>", Escape("        <ignoredTypes>"));
            content.Append("<div class=\"code-ignoredTypes-options-tabs code-ignoredTypes-options\"></div>");
            content.AppendFormat("<pre>{0}</pre>", Escape("        </ignoredTypes>"));
            content.Append("</div>");
            content.AppendFormat("<pre>{0}</pre></div>", Escape("    </tabs>"));

            content.AppendFormat("<pre class=\"code-comment code-content code-section-runtimePolicies-off\">{0}</pre>", Escape("    <!-- Runtime Policy Management\n    <runtimePolicies>\n        <ignoredTypes>\n            <add type=\"{Namespace.Type, AssemblyName}\"/>\n        </ignoredTypes>\n    </runtimePolicies>\n    -->"));

            content.AppendFormat("<div class=\"code-content code-section-runtimePolicies code-section-child\" data-altState=\"code-section-runtimePolicies-off\"><pre>{0}</pre>", Escape("    <runtimePolicies>"));
            content.Append("<div class=\"code-runtimePolicies-ignoredTypes\">");
            content.AppendFormat("<pre>{0}</pre>", Escape("        <ignoredTypes>"));
            content.Append("<div class=\"code-ignoredTypes-options-runtimePolicies code-ignoredTypes-options\"></div>");
            content.AppendFormat("<pre>{0}</pre>", Escape("        </ignoredTypes>"));
            content.Append("</div>");
            content.Append("<div class=\"code-runtimePolicies-blacklist configuration-options-item\">");
            content.AppendFormat("<pre>{0}</pre>", Escape("        <uris>\n            <add regex=\".*/admin.*\"/>\n        </uris>"));
            content.Append("</div>");
            content.Append("<div class=\"code-runtimePolicies-statusCodes configuration-options-item\">");
            content.AppendFormat("<pre>{0}</pre>", Escape("        <statusCodes>\n            <add statusCode=\"404\"/>\n        </statusCodes>"));
            content.Append("</div>");
            content.Append("<div class=\"code-runtimePolicies-contentTypes configuration-options-item\">");
            content.AppendFormat("<pre>{0}</pre>", Escape("        <contentTypes>\n            <add contentType=\"application/xml\"/>\n        </contentTypes>"));
            content.Append("</div>");
            content.AppendFormat("<pre>{0}</pre></div>", Escape("    </runtimePolicies>"));

            content.AppendFormat("<div class=\"code-content code-section-inspectors code-section-child\"><pre>{0}</pre>", Escape("    <inspectors>"));
            content.Append("<div class=\"code-tabs-ignoredTypes\">");
            content.AppendFormat("<pre>{0}</pre>", Escape("        <ignoredTypes>"));
            content.Append("<div class=\"code-ignoredTypes-options-inspectors code-ignoredTypes-options\"></div>");
            content.AppendFormat("<pre>{0}</pre>", Escape("        </ignoredTypes>"));
            content.Append("</div>");
            content.AppendFormat("<pre>{0}</pre></div>", Escape("    </inspectors>"));

            content.AppendFormat("<pre class=\"code-content\">{0}</pre>", Escape("</glimpse>"));

            content.Append("</div>");
            content.Append("<ul class=\"configuration-options\">");
            content.Append("<li>Tab Management - Individual Tabs can be disabled by instructing Glimpse to ignore their types</li>");
            content.Append("<li>Runtime Policy Management - Individual Policies can be disabled by instructing Glimpse to ignore their types<ul>");
            content.Append("<li><input type=\"checkbox\" data-show=\"code-runtimePolicies-contentTypes\">Content Types - Filter what specific Content Types Glimpse will be enabled for</li>");
            content.Append("<li><input type=\"checkbox\" data-show=\"code-runtimePolicies-blacklist\">Blacklist URIs - Filter Glimpse to only work on URIs that match specifc regular expressions</li>");
            content.Append("<li><input type=\"checkbox\" data-show=\"code-runtimePolicies-statusCodes\">Status Codes - Filter what specific Status Codes Glimpse will be enabled for</li></ul>");
            content.Append("<li><input type=\"checkbox\" data-show=\"code-logging\">Logging - An internal Glimpse diagnostics log can be enabled to help troubleshoot problems with Glimpse</li>");
            content.Append("</ul><div style=\"text-align:center\"><a href=\"http://getglimpse.com/Help/Configuration\">find out more online</a></div>");

            content.Append("</div>");

            content.Append("<div class=\"side-bar\">");
            //Cookie status on/off/other
            content.Append("<script type=\"text/javascript\"> var getCookie = function(name) { var re = new RegExp(name + \"=([^;]+)\"); var value = re.exec(document.cookie); return (value != null) ? unescape(value[1]) : null; }; var policy = getCookie('glimpsePolicy'); if (policy == 'On') { document.write(\"<div class='notification notification-success'><strong>Glimpse cookie set 'On'</strong> When you go back to your site, depending on your policies, you should see Glimpse at the bottom right of the page.</div>\"); } else if (policy == '' || policy == null) { document.write(\"<div class='notification notification-fail'><strong>Glimpse cookie set 'Off'</strong> By default for Glimpse to be visible, the `glimpsePolicy` cookie needs to be set 'On'. We have made this simple by providing the above buttons :D</div>\"); } else { document.write(\"<div class='notification notification-warn'><strong>Glimpse cookie set '\" + policy + \"'</strong> Looks like you are doing something custom. We don't have any specific buttons for your case, but if you ever want to just turn Glimpse On or Off simply use the above buttons.</div>\"); }</script>");
            //Configuration help
            content.Append("<div class=\"more-help\"><h3>Configuration Help?</h3>Want to learn more about configuring Glimpse or how to disable Tabs or Policies?<br /><a href=\"javascript:return true;\" class=\"config-open button button-small\">Start Config Helper</a></div>");  //head over to our <a href=\"http://getglimpse.com/Help/Configuration\" target=\"new\">config help page</a>
            //Logging help
            content.Append("<div class=\"more-help\"><h3>Glimpse Logging?</h3>Have an issue? help us figure it out by turning on logging.<br /><a href=\"javascript:return true;\" class=\"logging-on button button-small\">Find Out How</a></div>"); 
            content.Append("</div>");

            //Registered  Tabs
            content.Append("<h3 class=\"more-detail\">Standard Settings</h3>");
            content.Append("<ul class=\"root\"><li><strong>Registered  Tabs</strong>:<ul class=\"code-ignoredTypes-controller\" data-link=\"code-ignoredTypes-options-tabs\">");
            GroupContent(content, (x, y) => x.AppendFormat("<li><input type=\"checkbox\" data-type=\"{1}, {3}\" checked=\"checked\" /><strong>{0}</strong> - <span class=\"code\">{1}</span><span style=\"display:none\" class=\"more-detail\"> - <em>{2}</em></span></li>", y.Name, y.GetType().FullName, y.ExecuteOn, y.GetType().Assembly.GetName().Name), configuration.Tabs.OrderBy(x => x.Name), packages);
            content.Append("</ul>Want to create your own Tabs - <a href=\"http://getglimpse.com/Help/Custom-Tabs\" target=\"new\">see here!</a>");
            
            //Runtime Policies
            content.Append("</li><li><strong>Runtime Policies</strong>: <ul class=\"code-ignoredTypes-controller\" data-link=\"code-ignoredTypes-options-runtimePolicies\">");
            GroupContent(content, (x, y) => {
                    var warning = string.Empty;
                    if (y.GetType().FullName == "Glimpse.AspNet.Policy.LocalPolicy")
                    {
                        warning = "<strong class=\"warn\">*This policy means that Glimpse won't run remotely.*</strong>";
                    }

                    x.AppendFormat("<li><input type=\"checkbox\" data-type=\"{0}, {3}\" checked=\"checked\" /><span class=\"code\">{0}</span><span style=\"display:none\" class=\"more-detail\"> - <em>{1}</em></span> {2}</li>", y.GetType().FullName, y.ExecuteOn, warning, y.GetType().Assembly.GetName().Name);
                }, configuration.RuntimePolicies.OrderBy(x => x.GetType().FullName), packages);
            content.Append("</ul>Learn how to create your own policies - <a href=\"http://getglimpse.com/Help/Custom-Runtime-Policy\" target=\"new\">see here!</a></li></ul>");

            //Toggle details
            content.Append("<a class=\"more-detail\" href=\"javascript:return true;\" onclick=\"toggleClass('more-detail')\" style=\"display:block\">More details?</a><a class=\"more-detail\" href=\"javascript:return true;\" onclick=\"toggleClass('more-detail')\" style=\"display:none\">Less details?</a><div class=\"more-detail\" style=\"display:none\">");

            //Detailed Settings
            content.Append("<h3>Detailed Settings:</h3><ul class=\"root\"><li><strong>Inspectors</strong>: <ul class=\"code-ignoredTypes-controller\" data-link=\"code-ignoredTypes-options-inspectors\">");
            GroupContent(content, (x, y) => x.AppendFormat("<li><input type=\"checkbox\" data-type=\"{0}, {1}\" checked=\"checked\" /><span class=\"code\">{0}</span></li>", y.GetType().FullName, y.GetType().Assembly.GetName().Name), configuration.Inspectors.OrderBy(x => x.GetType().FullName), packages);

            //Resources
            content.Append("</ul></li><li><strong>Resources</strong>: <ul>");
            GroupContent(content, (x, y) => {
                    var paramaters = string.Empty;
                    if (y.Parameters != null)
                    {
                        paramaters = string.Join(", ", y.Parameters.Select(parameter => string.Format("{0} ({1})", parameter.Name, parameter.IsRequired)).ToArray());
                    }

                    var duplicate = string.Empty;
                    if (duplicateResources.Contains(y.Name))
                    {
                        duplicate = " <strong class=\"warn\">*Duplicate*</strong>";
                    }

                    content.AppendFormat("<li><strong>{0}</strong> - <span class=\"code\">{1}</span> - <em>{2}</em> {3}</li>", y.Name, y.GetType().FullName, paramaters, duplicate);
                }, 
                configuration.Resources.OrderBy(x => x.Name), packages);

            //Client Scripts
            content.Append("</ul></li><li><strong>Client Scripts</strong>: <ul>");
            GroupContent(content, (x, y) => x.AppendFormat("<li><span class=\"code\">{0}</span> - {1}</li>", y.GetType().FullName, y.Order), configuration.ClientScripts.OrderBy(x => x.GetType().FullName), packages);
            
            //More Details 
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

            //Registered Packages
            content.Append("<h3>Registered Packages:</h3>");
            content.Append("<p>NOTE, doesn't represent all the Glimpse dependent NuGet packages you have installed, just the ones that have registered as a NuGet package</p>");
            content.Append("<ul>");
            var nuGetPackageDiscoveryResult = NuGetPackageDiscoverer.Discover();
            foreach (var registeredPackage in nuGetPackageDiscoveryResult.FoundNuGetPackages)
            {
                content.AppendFormat("<li>{0} - {1}</li>", registeredPackage.GetId(), registeredPackage.GetVersion());
            }
            content.AppendFormat("</ul>");

            if (nuGetPackageDiscoveryResult.NonProcessableAssemblies.Length != 0)
            {
                content.Append("<strong class=\"warn\">The following assemblies could not be processed during NuGet package discovery. Check log for more details.</strong>");
                content.Append("<ul>");
                foreach (var nonProcessableAssembly in nuGetPackageDiscoveryResult.NonProcessableAssemblies)
                {
                    content.AppendFormat("<li>{0}</li>", nonProcessableAssembly.FullName);
                }
                content.AppendFormat("</ul>");
            }
        
            content.Append("</div>");
            content.Append("</div>");
            content.Append("<footer><div class=\"inner\"><p class=\"center\">For more info see <a href=\"http://getglimpse.com\">getGlimpse.com</a></p><div class=\"center\"><img src=\"" + githubDataUri + "\"> Found an <em>error</em>? <a href=\"https://github.com/glimpse/glimpse/issues\">Help us improve</a>.   <img src=\"" + twitterDataUri + "\"> Have a <em>question</em>? <a href=\"http://twitter.com/#search?q=%23glimpse\">Tweet us using #glimpse</a>.</div></div></div></footer>");
            content.Append("<script src=\"//ajax.googleapis.com/ajax/libs/jquery/2.0.2/jquery.min.js\"></script>");
            content.Append("<script>$(function() { $('.logging-on').click(function() {  $('.configuration-holder').show(); $('[data-show=\"code-logging\"]').click() }); $('.configuration-holder .close-section').click(function(){ $('.configuration-holder').hide(); }); $('.config-open').click(function() { $('.configuration-holder').toggle(); });  $('.configuration-options input').change(function() { $('.'+ $(this).attr('data-show')).toggle(); adjustConfigState(); });  $('.code-ignoredTypes-controller input').change(function() { $('.configuration-holder').show(); var item=$(this),itemType=item.attr('data-type'),infoSection=item.closest('.code-ignoredTypes-controller'),configSection=$('.'+infoSection.attr('data-link')),ignoreItem=configSection.find(\"pre[data-type='\"+itemType+\"']\");if(ignoreItem.length==0){configSection.append('<pre data-type=\"'+itemType+'\">            &lt;add type=\"'+itemType+'\" /></pre>')}else{ignoreItem.remove()}adjustConfigState(); }); var adjustConfigState=function(){$('.code-ignoredTypes-options').each(function(){var holder=$(this),parent=holder.parent();if(holder.find('pre').length==0){parent.hide()}else{parent.show()}});$('.code-section-child').each(function(){var holder=$(this),alt=holder.attr('data-altState');holder.show();if(holder.find('div:visible').length==0){holder.hide();if(alt){$('.'+alt).show()}}else{holder.show();if(alt){$('.'+alt).hide()}}})}; });</script>");
            content.Append("</body></html>");
             
            return new HtmlResourceResult(content.ToString());
        }

        private void GroupContent<T>(StringBuilder content, Action<StringBuilder, T> action, IEnumerable<T> items, IDictionary<string, PackageItemDetail> packages)
        {
            var groupedItems = GroupItems(items, packages);
            foreach (var group in groupedItems)
            {
                var package = group.Value;
                content.AppendFormat("<li><strong>{0}</strong> <span class=\"package-version\">{1}</span><ul>", package.Package.Name, !string.IsNullOrEmpty(package.Package.Version) ? string.Format("({0})", package.Package.Version) : string.Empty);
                foreach (var item in package.Items)
                {
                    action(content, item);
                }

                content.Append("</ul></li>");
            }
        }

        private IDictionary<string, PackageItem<T>> GroupItems<T>(IEnumerable<T> items, IDictionary<string, PackageItemDetail> packages)
        {
            var result = new SortedDictionary<string, PackageItem<T>>();
            var otherPackage = new PackageItemDetail() { Name = "Other", Assembly = string.Empty };

            if (items != null)
            {
                foreach (var item in items)
                {
                    var package = (PackageItemDetail)null;
                    if (!packages.TryGetValue(item.GetType().Assembly.FullName, out package))
                    {
                        package = otherPackage;
                    }

                    var entry = (PackageItem<T>)null;
                    if (!result.TryGetValue(package.Assembly, out entry))
                    {
                        entry = result[package.Assembly] = new PackageItem<T>();
                        entry.Package = package;
                    }

                    entry.Items.Add(item);
                }
            }

            return result;
        }

        private IDictionary<string, PackageItemDetail> FindPackages()
        {
            var result = new Dictionary<string, PackageItemDetail>();
            var packages = NuGetPackage.GetRegisteredPackages();

            foreach (var package in packages)
            {
                var name = package.GetAssemblyName();
                result[name] = new PackageItemDetail { Name = package.GetId(), Version = package.GetVersion(), Assembly = name };
            }

            return result;
        } 

        private IEnumerable<string> DetectDuplicateResources(IEnumerable<IResource> resources)
        { 
            return resources.GroupBy(x => x.Name).Where(x => x.Count() > 1).Select(x => x.Key); 
        }

        private string Escape(string content)
        {
            return content.Replace("<", "&lt;");
        }

        private class PackageItem<T>
        {
            public PackageItem()
            { 
                Items = new List<T>();
            }

            public PackageItemDetail Package { get; set; }

            public IList<T> Items { get; private set; }
        }

        private class PackageItemDetail
        {
            public string Name { get; set; }

            public string Version { get; set; }

            public string Assembly { get; set; } 
        } 

        private class GroupItemsComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == string.Empty)
                {
                    return y == string.Empty ? 0 : 1;
                }

                if (y == string.Empty)
                {
                    return -1;
                }

                // Change this comparer if required.
                return StringComparer.OrdinalIgnoreCase.Compare(x, y);
            }
        }
    } 
}