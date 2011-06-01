using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Plumbing;
using Glimpse.Core.Sanitizer;
using Glimpse.Core.Validator;
using Glimpse.Core.Warning;
using Environment = Glimpse.Core.Configuration.Environment;

namespace Glimpse.Core
{
    public class Module : IHttpModule
    {
        private static GlimpseConfiguration Configuration { get; set; }
        private static CompositionContainer Container { get; set; }
        private static BlacklistedSafeDirectoryCatalog DirectoryCatalog { get; set; }
        private static GlimpseRequestValidator RequestValidator { get; set; }
        private static IGlimpseSanitizer Sanitizer { get; set; }//TODO: new up via config

        [ImportMany]
        private static IEnumerable<IGlimpseHandler> Handlers { get; set; }

        [Export]
        internal static GlimpseSerializer Serializer { get; set; }

        [ImportMany]
        internal static IEnumerable<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        static Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ??
                            new GlimpseConfiguration();

            //TODO: abort if config is off?

            DirectoryCatalog = new BlacklistedSafeDirectoryCatalog("bin", Configuration.PluginBlacklist.TypeNames());
            Container = new CompositionContainer(DirectoryCatalog);

            RequestValidator = new GlimpseRequestValidator(Configuration, Enumerable.Empty<IGlimpseValidator>());

            Sanitizer = new CSharpSanitizer();

            Serializer = new GlimpseSerializer();

            Handlers = Enumerable.Empty<IGlimpseHandler>();
            Plugins = Enumerable.Empty<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();
        }

        public void Init(HttpApplication context)
        {
            if (!Configuration.Enabled) return; //Do nothing if Glimpse is off, events are not wired up

            if (Plugins.Count() == 0)
            {
                lock (Plugins)
                {
                    if (Plugins.Count() == 0)
                    {
                        var contextBase = new HttpContextWrapper(context.Context);

                        ComposePlugins(contextBase); //Have MEF satisfy our needs

                        //Allow plugin's registered for Intialization to setup
                        foreach (var plugin in Plugins.Where(plugin => plugin.Metadata.ShouldSetupInInit))
                        {
                            plugin.Value.SetupInit();
                        }
                    }
                }
            }

            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
            context.PostRequestHandlerExecute += OnPostRequestHandlerExecute;
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
            context.PostMapRequestHandler += OnPostMapRequestHandler;
        }

        #region Event Handlers
        private static void OnEndRequest(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;

            if (httpApplication != null)
                EndRequest(new HttpContextWrapper(httpApplication.Context));
        }

        private static void OnPostMapRequestHandler(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;

            if (httpApplication != null)
                PostMapRequestHandler(new HttpContextWrapper(httpApplication.Context));
        }

        private static void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;

            if (httpApplication != null)
                PreSendRequestHeaders(new HttpContextWrapper(httpApplication.Context));
        }

        private static void OnPostRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;

            if (httpApplication != null)
                PostRequestHandlerExecute(new HttpContextWrapper(httpApplication.Context));
        }

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;

            if (httpApplication != null)
                BeginRequest(new HttpContextWrapper(httpApplication.Context));
        }
        #endregion Event Handlers

        private static void BeginRequest(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.BeginRequest)) return;

            context.InitGlimpseContext();
        }

        private static void PostMapRequestHandler(HttpContextBase context)
        {
            context.Items[GlimpseConstants.ValidPath] = false;

            var pathSegments = context.Request.Path.Split('/');
            var i = Array.FindIndex(pathSegments,
                                    segment =>
                                    segment.Equals(Configuration.RootUrlPath, StringComparison.CurrentCultureIgnoreCase));
            if (i > -1 && i < pathSegments.Length - 1) //Make sure key was found, and not the last element of segments
            {
                var resourceName = pathSegments[i + 1];
                var handler =
                    Handlers.Where(h => h.ResourceName.Equals(resourceName, StringComparison.CurrentCultureIgnoreCase)).
                        FirstOrDefault();

                if (handler != null)
                {
                    context.Handler = handler;
                    context.Items[GlimpseConstants.ValidPath] = true;
                }
            }
        }

        private static void PostRequestHandlerExecute(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PostRequestHandlerExecute)) return;

            ProcessData(context, true); //Run all plugins that DO need access to Session
        }

        private static void EndRequest(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.EndRequest)) return;

            ProcessData(context, false); //Run all plugins that DO NOT need access to Session
        }

        private static void PreSendRequestHeaders(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PreSendRequestHeaders)) return;

            var requestId = Guid.NewGuid();

            var json = GenerateGlimpseOutput(context, requestId);

            Persist(json, context, requestId);
        }

        public void Dispose()
        {
/*
            if (Container != null)
                Container.Dispose();
*/
        }

        private void ComposePlugins(HttpContextBase context)
        {
            var batch = new CompositionBatch();

            Container.ComposeParts(this, RequestValidator);

            Container.Compose(batch);

            Plugins = Container.GetExports<IGlimpsePlugin, IGlimpsePluginRequirements>();
            Handlers = Container.GetExportedValues<IGlimpseHandler>();
            Serializer.AddConterers(Container.GetExportedValues<IGlimpseConverter>());

            var store = context.GetWarnings();
            store.AddRange(DirectoryCatalog.Exceptions.Select(exception => new ExceptionWarning(exception)));
        }

        private static void Persist(string json, HttpContextBase context, Guid requestId)
        {
            if (Configuration.RequestLimit <= 0) return;

            var store = context.Application;

            //TODO: Turn Queue into provider model so it can be stored in SQL/Caching layer for farms
            var queue = store[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;

            if (queue == null)
                store[GlimpseConstants.JsonQueue] =
                    queue = new Queue<GlimpseRequestMetadata>(Configuration.RequestLimit);

            if (queue.Count == Configuration.RequestLimit) queue.Dequeue();

            var browser = context.Request.Browser;
            queue.Enqueue(new GlimpseRequestMetadata
                              {
                                  Browser = string.Format("{0} {1}", browser.Browser, browser.Version),
                                  ClientName = context.GetClientName(),
                                  Json = json,
                                  RequestTime = DateTime.Now.ToLongTimeString(),
                                  RequestId = requestId,
                                  IsAjax = context.IsAjax().ToString(),
                                  Url = context.Request.RawUrl,
                                  Method = context.Request.HttpMethod
                              });
        }

        private static void ProcessData(HttpContextBase context, bool sessionRequired)
        {
            IDictionary<string, object> data;
            if (!context.TryGetData(out data)) return;

            lock (Plugins)
            {
                foreach (var plugin in Plugins.Where(p => p.Metadata.SessionRequired == sessionRequired))
                {
                    var p = plugin.Value;
                    try
                    {
                        var pluginData = p.GetData(context);
                        data.Add(p.Name, pluginData);
                    }
                    catch (Exception ex)
                    {
                        data.Add(p.Name, ex.Message);
                    }
                }
            }
        }

        private static string GenerateGlimpseOutput(HttpContextBase context, Guid requestId)
        {
            IDictionary<string, object> data;
            if (!context.TryGetData(out data)) return "Error: No Glimpse Data Found";

            string json = CreateJsonPayload(data, context);

            json = Sanitizer.Sanitize(json);

            AppendToResponse(context, json, requestId);

            return json;
        }

        private static void AppendToResponse(HttpContextBase context, string json, Guid requestId)
        {
            if (context.IsAjax())
            {
                context.Response.AddHeader(GlimpseConstants.HttpHeader, requestId.ToString());
            }
            else
            {
                if (context.GetGlimpseMode() == GlimpseMode.On)
                {
                    var path = VirtualPathUtility.ToAbsolute("~/", context.Request.ApplicationPath);
                    var html = string.Format(@"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{1}'>var glimpse = {0}, glimpsePath = '{2}';</script>", json, requestId, path);
                    html += @"<script type='text/javascript' id='glimpseClient' src='" + UrlCombine(path, Configuration.RootUrlPath, "glimpseClient.js") + "'></script>";
                    context.Response.Write(html);//TODO: Use a filter and put this inside </body>
                }
            }
        }

        //TODO: clean up this massive method
        private static string CreateJsonPayload(IDictionary<string, object> data, HttpContextBase context)
        {
            var warnings = context.GetWarnings();

            var sb = new StringBuilder("{");
            foreach (var item in data)
            {
                try
                {
                    string dataString = Serializer.Serialize(item.Value);
                    sb.Append(string.Format("\"{0}\":{1},", item.Key, dataString));
                }
                catch (Exception ex)
                {
                    var message = Serializer.Serialize(ex.Message);
                    message = message.Remove(message.Length - 1).Remove(0, 1);
                    var callstack = Serializer.Serialize(ex.StackTrace);
                    callstack = callstack.Remove(callstack.Length - 1).Remove(0, 1);
                    const string helpMessage =
                        "Please implement an IGlimpseConverter for the type mentioned above, or one of its base types, to fix this problem. More info on a better experience for this coming soon, keep an eye on <a href='http://getGlimpse.com' target='main'>getGlimpse.com</a></span>";

                    sb.Append(
                        string.Format(
                            "\"{0}\":\"<span style='color:red;font-weight:bold'>{1}</span><br/>{2}</br><span style='color:black;font-weight:bold'>{3}</span>\",",
                            item.Key, message, callstack, helpMessage));
                }
            }

            //Add exceptions tab if needed
/*            if (warnings.Count > 0)
            {
                var warningTable = new List<object[]> {new[] {"Type", "Message"}};
                warningTable.AddRange(warnings.Select(warning => new[] {warning.GetType().Name, warning.Message}));

                var dataString = JsonConvert.SerializeObject(warningTable, DefaultFormatting);
                sb.Append(string.Format("\"{0}\":{1},", "GlimpseWarnings", dataString));
            }*/

            if (sb.Length > 1) sb.Remove(sb.Length - 1, 1);

            var requestMetadata = new Dictionary<string, object>();
            var pluginsMetadata = new Dictionary<string, object>();
            var metadata = new Dictionary<string, object>
                               {
                                   {"request", requestMetadata},
                                   {"plugins", pluginsMetadata},
                               };
            //request specific metadata
            var environmentUrls = new Dictionary<string, string>();
            foreach (Environment environment in Configuration.Environments)
            {
                environmentUrls.Add(environment.Name, environment.Something(context.Request.Url).ToString());
            }

            requestMetadata.Add("environmentUrls", environmentUrls);
            requestMetadata.Add("runningVersion", decimal.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString(2)));
            

            //plugin specific metadata);
            foreach (var plugin in Plugins)
            {
                var pluginData = new Dictionary<string, object>();

                var pluginValue = plugin.Value;

                var helpPlugin = pluginValue as IProvideGlimpseHelp;
                if (helpPlugin != null) pluginData.Add("helpUrl", helpPlugin.HelpUrl);

                if (pluginData.Count > 0) pluginsMetadata.Add(pluginValue.Name, pluginData);
            }

            var metadataString = Serializer.Serialize(metadata);
            sb.Append(string.Format(",\"{0}\":{1},", "_metadata", metadataString));
            if (sb.Length > 1) sb.Remove(sb.Length - 1, 1);


            sb.Append("}");

            return sb.ToString();
        }

        private static string UrlCombine(params string[] segments)
        {
            if (segments.Length == 0) return string.Empty;
            if (segments.Length == 1) return segments[0];

            var stringBuilder = new StringBuilder(segments[0]);

            for (int i = 1; i < segments.Length; i++)
            {
                if (!segments[i - 1].EndsWith("/") && !segments[i].StartsWith("/"))
                    stringBuilder.Append("/");

                stringBuilder.Append(segments[i]);
            }

            return stringBuilder.ToString().Replace("//", "/");
        }
    }
}