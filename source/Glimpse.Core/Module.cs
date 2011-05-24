using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Plumbing;
using Glimpse.Core.Sanitizer;
using Glimpse.Core.Validator;
using Glimpse.Core.Warning;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Environment = Glimpse.Core.Configuration.Environment;

namespace Glimpse.Core
{
    public class Module : IHttpModule
    {
        private static GlimpseConfiguration Configuration { get; set; }
        private static CompositionContainer Container { get; set; }
        private const Formatting DefaultFormatting = Formatting.None;
        private static BlacklistedSafeDirectoryCatalog DirectoryCatalog { get; set; }
        private static GlimpseRequestValidator RequestValidator { get; set; }
        private static IGlimpseSanitizer Sanitizer { get; set; }

        [ImportMany]
        private static IEnumerable<IGlimpseHandler> Handlers { get; set; }

        [ImportMany]
        private static IEnumerable<IGlimpseConverter> JsConverters { get; set; }

        [Export]
        internal static JsonSerializerSettings JsonSerializerSettings { get; set; }

        [ImportMany]
        internal static IEnumerable<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        static Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ??
                            new GlimpseConfiguration();

            DirectoryCatalog = new BlacklistedSafeDirectoryCatalog("bin", Configuration.PluginBlacklist.TypeNames());
            Container = new CompositionContainer(DirectoryCatalog);

            RequestValidator = new GlimpseRequestValidator(Configuration, Enumerable.Empty<IGlimpseValidator>());

            Sanitizer = new CSharpSanitizer();

            JsonSerializerSettings = new JsonSerializerSettings {ContractResolver = new GlimpseContractResolver()};
            JsonSerializerSettings.Error += (obj, args) =>
                                                {
                                                    var warnings = HttpContext.Current.GetWarnings();
                                                    warnings.Add(new SerializationWarning(args.ErrorContext.Error));
                                                    args.ErrorContext.Handled = true;
                                                };

            Handlers = Enumerable.Empty<IGlimpseHandler>();
            JsConverters = Enumerable.Empty<IGlimpseConverter>();
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
                        ComposePlugins(context); //Have MEF satisfy our needs

                        //Allow plugin's registered for Intialization to setup
                        foreach (var plugin in Plugins.Where(plugin => plugin.Metadata.ShouldSetupInInit))
                        {
                            plugin.Value.SetupInit(context);
                        }
                    }
                }
            }

            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
            context.PostRequestHandlerExecute += PostRequestHandlerExecute;
            context.PreSendRequestHeaders += PreSendRequestHeaders;
            context.PostMapRequestHandler += PostMapRequestHandler;
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            //HttpApplication httpApplication;
            //if (!sender.IsValidRequest(out httpApplication, Configuration, false, false)) return;

            var httpApplication = sender as HttpApplication;
            if (!RequestValidator.IsValid(httpApplication, LifecycleEvent.BeginRequest)) return;
/*
            var responder = Responders.GetResponderFor(httpApplication);
            if (responder != null)
            {
                responder.Respond(httpApplication, Configuration);
                return;
            }
*/

            httpApplication.InitGlimpseContext();
        }

        private static void PostMapRequestHandler(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            if (application == null) return;

            application.Context.Items[GlimpseConstants.ValidPath] = false;

            var pathSegments = application.Request.Path.Split('/');
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
                    application.Context.Handler = handler;
                    application.Context.Items[GlimpseConstants.ValidPath] = true;
                }
            }
        }

        private static void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            //HttpApplication httpApplication;
            //if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;


            var httpApplication = sender as HttpApplication;
            if (!RequestValidator.IsValid(httpApplication, LifecycleEvent.PostRequestHandlerExecute)) return;

            ProcessData(httpApplication, true); //Run all plugins that DO need access to Session
        }

        private static void EndRequest(object sender, EventArgs e)
        {
            //HttpApplication httpApplication;
            //if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            var httpApplication = sender as HttpApplication;
            if (!RequestValidator.IsValid(httpApplication, LifecycleEvent.EndRequest)) return;

            ProcessData(httpApplication, false); //Run all plugins that DO NOT need access to Session
        }

        private static void PreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpApplication httpApplication;
            //if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            var httpApplication = sender as HttpApplication;
            if (!RequestValidator.IsValid(httpApplication, LifecycleEvent.PreSendRequestHeaders)) return;

            var requestId = Guid.NewGuid();

            var json = GenerateGlimpseOutput(httpApplication, requestId);

            Persist(json, httpApplication, requestId);
        }

        public void Dispose()
        {
/*
            if (Container != null)
                Container.Dispose();
*/
        }

        private void ComposePlugins(HttpApplication application)
        {
            var batch = new CompositionBatch();

            Container.ComposeParts(this, RequestValidator);

            Container.Compose(batch);

            Plugins = Container.GetExports<IGlimpsePlugin, IGlimpsePluginRequirements>();
            Handlers = Container.GetExportedValues<IGlimpseHandler>();
            JsConverters = Container.GetExportedValues<IGlimpseConverter>();

            var store = application.Context.GetWarnings();
            store.AddRange(DirectoryCatalog.Exceptions.Select(exception => new ExceptionWarning(exception)));

            //wireup converters into serializer
            var converters = JsonSerializerSettings.Converters;
            foreach (var jsConverter in JsConverters)
            {
                converters.Add(new JsonConverterToIGlimpseConverterAdapter(jsConverter));
            }
            converters.Add(new JavaScriptDateTimeConverter());//adds a more human readable datetime format
        }

        private static void Persist(string json, HttpApplication ctx, Guid requestId)
        {
            if (Configuration.RequestLimit <= 0) return;

            var store = ctx.Application;

            //TODO: Turn Queue into provider model so it can be stored in SQL/Caching layer for farms
            var queue = store[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;

            if (queue == null)
                store[GlimpseConstants.JsonQueue] =
                    queue = new Queue<GlimpseRequestMetadata>(Configuration.RequestLimit);

            if (queue.Count == Configuration.RequestLimit) queue.Dequeue();

            var browser = ctx.Request.Browser;
            queue.Enqueue(new GlimpseRequestMetadata
                              {
                                  Browser = string.Format("{0} {1}", browser.Browser, browser.Version),
                                  ClientName = ctx.GetClientName(),
                                  Json = json,
                                  RequestTime = DateTime.Now.ToLongTimeString(),
                                  RequestId = requestId,
                                  IsAjax = ctx.IsAjax().ToString(),
                                  Url = ctx.Request.RawUrl,
                                  Method = ctx.Request.HttpMethod
                              });
        }

        private static void ProcessData(HttpApplication httpApplication, bool sessionRequired)
        {
            IDictionary<string, object> data;
            if (!httpApplication.TryGetData(out data)) return;

            lock (Plugins)
            {
                foreach (var plugin in Plugins.Where(p => p.Metadata.SessionRequired == sessionRequired))
                {
                    var p = plugin.Value;
                    try
                    {
                        var pluginData = p.GetData(httpApplication);
                        data.Add(p.Name, pluginData);
                    }
                    catch (Exception ex)
                    {
                        data.Add(p.Name, ex.Message);
                    }
                }
            }
        }

        private static string GenerateGlimpseOutput(HttpApplication application, Guid requestId)
        {
            IDictionary<string, object> data;
            if (!application.TryGetData(out data)) return "Error: No Glimpse Data Found";

            string json = CreateJsonPayload(data, application);

            json = Sanitizer.Sanitize(json);

            AppendToResponse(application, json, requestId);

            return json;
        }

        private static void AppendToResponse(HttpApplication application, string json, Guid requestId)
        {
            //if ajax request, render glimpse data to headers
            if (application.IsAjax())
            {
                application.Response.AddHeader(GlimpseConstants.HttpHeader, requestId.ToString());
            }
            else
            {
                if (application.GetGlimpseMode() == GlimpseMode.On)
                {
                    var path = VirtualPathUtility.ToAbsolute("~/", application.Context.Request.ApplicationPath);
                    var html =
                        string.Format(
                            @"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{1}'>var glimpse = {0}, glimpsePath = '{2}';</script>",
                            json, requestId, path);
                    html += @"<script type='text/javascript' id='glimpseClient' src='" +
                            UrlCombine(path, Configuration.RootUrlPath, "glimpseClient.js") + "'></script>";
                    application.Response.Write(html);
                }
            }
        }

        private static string CreateJsonPayload(IDictionary<string, object> data, HttpApplication application)
        {
            var warnings = application.Context.GetWarnings();

            var sb = new StringBuilder("{");
            foreach (var item in data)
            {
                try
                {
                    string dataString = JsonConvert.SerializeObject(item.Value, DefaultFormatting,
                                                                    JsonSerializerSettings);
                    sb.Append(string.Format("\"{0}\":{1},", item.Key, dataString));
                }
                catch (Exception ex)
                {
                    var message = JsonConvert.SerializeObject(ex.Message, DefaultFormatting, JsonSerializerSettings);
                    message = message.Remove(message.Length - 1).Remove(0, 1);
                    var callstack = JsonConvert.SerializeObject(ex.StackTrace, DefaultFormatting, JsonSerializerSettings);
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
                environmentUrls.Add(environment.Name, environment.Something(application.Context.Request.Url).ToString());
            }

            requestMetadata.Add("environmentUrls", environmentUrls);

            //plugin specific metadata);
            foreach (var plugin in Plugins)
            {
                var pluginData = new Dictionary<string, object>();

                var pluginValue = plugin.Value;

                var helpPlugin = pluginValue as IProvideGlimpseHelp;
                if (helpPlugin != null) pluginData.Add("helpUrl", helpPlugin.HelpUrl);

                if (pluginData.Count > 0) pluginsMetadata.Add(pluginValue.Name, pluginData);
            }

            var metadataString = JsonConvert.SerializeObject(metadata, DefaultFormatting, JsonSerializerSettings);
            sb.Append(string.Format(",\"{0}\":{1},", "_metadata", metadataString));
            if (sb.Length > 1) sb.Remove(sb.Length - 1, 1);


            sb.Append("}");

            return sb.ToString();
        }

        private static string UrlCombine(params string[] segments)
        {
            if (segments.Length == 0) return string.Empty;

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