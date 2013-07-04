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
            var packages = FindPacakges();

            content.Append("<!DOCTYPE html><html><head><title>Glimpse - Configuration Page</title><link rel=\"shortcut icon\" href=\"http://getglimpse.com/content/_v1/app-config-favicon.png?version=" + GlimpseRuntime.Version + "\" />");
            content.Append("<style>*, *:before, *:after{-webkit-box-sizing: border-box;-moz-box-sizing: border-box;-ms-box-sizing: border-box;-o-box-sizing: border-box;box-sizing: border-box;}body{margin: 0;font-family: \"Segoe UI Web Regular\",\"Segoe UI\",\"Helvetica Neue\",Helvetica,Arial serif;font-size: 1em;line-height: 1.6em;}pre{margin:0}.code{font-size: 1.45em;font-family:monospace;}h1, h2, h3{font-weight: normal;}header{color: #fff;background-color: #323d42;height: 450px;}header table{width: 100%;}header .detail{text-align: center;width: 250px;}header h2{position: relative;margin-bottom:0;}header .find-more{color: white;font-size: 0.8em;margin-top: 0; }header .find-more:hover,header .find-more:active{color: white;}.inner{margin:0 auto;max-width: 1200px;width: 80%;min-width: 900px;vertical-align: top;padding-top: 1em;}.button{width: 250px;line-height: 1.2em;margin: 0.5em auto;text-align: center;font-size: 24px;padding: 10px 41px;text-decoration: none;display: block;color: white;border: 3px solid white;background-color: #434f54;}.button:hover{background-color: #3f4a4f;}.button-small{font-size: 0.95em;}.inner .button{border-color: #ccc;}.message{font-size: 0.5em;line-height: 1em;width: 125px;left: -150px;top: 20px;position: absolute; font-style:italic;}img{border:0px;}.center{text-align: center;}.notification{ padding: 17px; font-size: 1.2em; text-align: center; }.notification-success{background-color: #B5CDA4; color: #486E25;}.notification-fail{background-color: #E4BBB1; color: #DA6953;}.notification-warn{background-color: #E4D1A9; color: #DA9221;}.version span{font-size:0.8em;font-style:italic;}.warn{color:red; font-weight:bold;}.package-version{font-size:0.7em;}.root > li {margin-bottom:15px;}.more-help h3{margin: 40px 0 5px; text-align: center;}.side-bar{float:right;width:250px;margin-top:22px;}.out-dated{padding-bottom:1px 0 18px;text-align:center;background-color: #FFF9B0;color: goldenrod;}.code-holder{padding:10px; margin:10px 0; background-color: #eee;font-family: monospace;line-height: 1.3em;font-size: 1.3em;}.close-section {  position: absolute; right: -19px; border-radius: 20px; font-weight: bold; height: 40px; width: 40px; text-align: center; font-size: 140%; line-height: 120%; margin-top: -9px; cursor: pointer; padding:0}.code-comment{color:#52AA00;}.code-content{margin-bottom:10px}.configuration-holder{display:none;position: relative}.description{text-align:center;text-align: center;font-size: 1.2em;font-weight: normal;}.description strong{text-decoration: underline;font-size: 1.1em;}.configuration-options-item,.code-section-child{display:none;}</style>");
            content.Append("<script type=\"text/javascript\">var toggleClass = function(name) { toggleItems(document.getElementsByClassName(name)); }, toggleItems = function(e) { for(var i = 0; i < e.length; i++) { toggleItem(e[i]); } }, toggleItem = function(e) { if(e.style.display == 'none') { e.style.display = ''; } else { e.style.display = 'none'; } };</script>");
            content.Append("</head><body>");
            content.Append("<header><div class=\"inner\"><table><tr><td class=\"logo\"><a href=\"http://getglimpse.com/\" title=\"Glimpse Home :D\"><img width=\"325\" src=\"http://getglimpse.com/content/_v1/app-config-logo.png?version=" + GlimpseRuntime.Version + "\" alt=\"Glimpse Home :D\"/></a></td><td class=\"detail\"><div class=\"version\">v" + GlimpseRuntime.Version + " <span>(core)</span></div><h2>Bookmarklets<div class=\"message\">“Drag us to your favorites bar for quick and easy access to Glimpse”</div></h2><a href=\"http://getglimpse.com/Help/First-Run#Glimpse-Bookmarklets\" class=\"find-more\" target=\"new\">find out more</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpsePolicy=On;path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Turn Glimpse On</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpsePolicy=;path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Turn Glimpse Off</a><a class=\"button\" href=\"javascript:(function(){document.cookie='glimpseId='+ prompt('Client Name?') +';path=/;expires=Sat, 01 Jan 2050 12:00:00 GMT;';window.location.reload();})();\">Set Glimpse Session Name</a></td></tr></table></div></header>");
            content.Append("<div class=\"inner\">");

            //Duplicate resource 
            var duplicateResources = DetectDuplicateResources(configuration.Resources);
            if (duplicateResources.Any())
            {
                var instances = string.Join(", ", duplicateResources.Select(x => string.Format("<strong>{0}</strong>", x)).ToArray());
                content.AppendFormat("<div class='notification notification-fail'><strong>Houston we have a problem :(</strong><br /> We have detected that the following resources have been duplicated - {0}. Typically when this happens, Glimpse will fail to operate correctly and lead to ambiguous references.</div>", instances);
            }

            //Update notification
            content.Append("<script type=\"text/javascript\">var getOptions = function() { var data = localStorage.getItem('glimpseOptions'); return data != null ? JSON.parse(data) : {}; }, options = getOptions(), currentHash = options.hash, checkUri = options.versionCheckUri, viewUri = options.versionViewUri, hasNewer = options.hasNewerVersion; if (hasNewer && checkUri.indexOf('hash=' + currentHash) > -1) { document.write(\"<div class='out-dated'><h3>Really sorry to have to say this but you are out of date :(</h3>A new version of Glimpse is waiting for you... its very lonely without you. It will be worth your while updating. <br /><a href='\" + viewUri + \"' target='new'>See what you are missing out on</a></div>\"); }</script>");

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

        private IDictionary<string, PackageItemDetail> FindPacakges()
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