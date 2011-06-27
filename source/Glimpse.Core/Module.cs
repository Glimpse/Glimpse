using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Globalization;
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
using Environment = Glimpse.Core.Configuration.Environment;

namespace Glimpse.Core
{
    public class Module : IHttpModule
    {
        internal static GlimpseRequestValidator RequestValidator { get; set; }
        private static IGlimpseSanitizer Sanitizer { get; set; }//TODO: new up via config

        [Export] public static GlimpseSerializer Serializer { get; set; }
        [Export] public static GlimpseConfiguration Configuration { get; set; }
        [Export] public static IGlimpseMetadataStore MetadataStore { get; set; }

        private static IGlimpseLogger Logger { get; set; }

        internal static IEnumerable<IGlimpseHandler> Handlers { get; set; }
        internal static IEnumerable<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        static Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();

            GlimpseFactory.Configuration = Configuration;
            Logger = GlimpseFactory.CreateLogger();

            Logger.Info(Configuration);

            RequestValidator = new GlimpseRequestValidator(Configuration, Enumerable.Empty<IGlimpseValidator>());

            Sanitizer = new CSharpSanitizer();

            Serializer = new GlimpseSerializer();

            Handlers = Enumerable.Empty<IGlimpseHandler>();

            Plugins = Enumerable.Empty<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();

            Logger.Info("Glimpse Module constructed");
        }

        public void Init(HttpApplication context)
        {
            if (!Configuration.Enabled) return; //Do nothing if Glimpse is off, events are not wired up

            MetadataStore = new InProcStackMetadataStore(Configuration, new HttpApplicationStateWrapper(context.Application));

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
                            Logger.Info("Calling SetupInit() on " + plugin.Value.GetType().FullName);
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

            Logger.Info("Glimpse Module Init Complete");
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

            Logger.Info("BeginRequest handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path+")");
        }

        internal static void PostMapRequestHandler(HttpContextBase context)
        {
            //temporary measure to help users move from glimpse/config to glimpse.asx
            if (context.Request.Path.ToLower().Contains(@"glimpse/config"))
            {
                Logger.Info("Noticed request for glimpse/config, redirecting");
                context.Response.RedirectPermanent(context.GlimpseResourcePath(null)+"?redirect=1", true);
            }
        }

        private static void PostRequestHandlerExecute(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PostRequestHandlerExecute)) return;

            ProcessData(context, true); //Run all plugins that DO need access to Session

            Logger.Info("PostRequestHandlerExecute handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
        }

        private static void EndRequest(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.EndRequest)) return;

            ProcessData(context, false); //Run all plugins that DO NOT need access to Session

            Logger.Info("EndRequest handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
        }

        private static void PreSendRequestHeaders(HttpContextBase context)
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PreSendRequestHeaders)) return;

            var jsonPayload = GenerateGlimpseOutput(context);
            Logger.Info("Glimpse output generated for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");

            MetadataStore.Persist(context.GetRequestMetadata(jsonPayload));
            Logger.Info("RequestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")" + " persisted");
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

            var directoryCatalog = new BlacklistedSafeDirectoryCatalog("bin", Configuration.PluginBlacklist.TypeNames());
            var container = new CompositionContainer(directoryCatalog);

            container.ComposeParts(this, RequestValidator);

            container.Compose(batch);

            Plugins = container.GetExports<IGlimpsePlugin, IGlimpsePluginRequirements>();
            Handlers = container.GetExportedValues<IGlimpseHandler>();
            var glimpseConverters = container.GetExportedValues<IGlimpseConverter>();
            Serializer.AddConverters(glimpseConverters);

            Logger.Info("MEF Parts composed: " + Plugins.Count() + " IGlimpsePlugins, " + Handlers.Count() + " IGlimpseHandlers and " + glimpseConverters.Count() + " IGlimpseConverters configured");

            foreach (var exception in directoryCatalog.Exceptions)
            {
                Logger.Warn("MEF Loading error", exception);
            }
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

        private static string GenerateGlimpseOutput(HttpContextBase context)
        {
            IDictionary<string, object> data;
            if (!context.TryGetData(out data)) return "Error: No Glimpse Data Found";

            string json = CreateJsonPayload(data, context);
            Logger.Info("Glimpse JSON payload created for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");

            json = Sanitizer.Sanitize(json);

            AppendToResponse(context, json);

            return json;
        }

        private static void AppendToResponse(HttpContextBase context, string json)
        {
            var requestId = context.GetGlimpseRequestId().ToString();

            if (context.IsAjax())
            {
                Logger.Info("Ajax request, adding HTTP Header for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
                context.Response.AddHeader(GlimpseConstants.HttpHeader, requestId);
            }
            else
            {
                if (context.GetGlimpseMode() == GlimpseMode.On)
                {
                    //var path = VirtualPathUtility.ToAbsolute("~/", context.Request.ApplicationPath);
                    var path = context.GlimpseResourcePath("");
                    var html = string.Format(@"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{1}'>var glimpse = {0}, glimpsePath = '{2}';</script>", json, requestId, path);
                    html += @"<script type='text/javascript' id='glimpseClient' src='" + context.GlimpseResourcePath("client.js") + "'></script>";
                    context.Response.Write(html);//TODO: Use a filter and put this inside </body>
                }
            }
        }

        //TODO: clean up this massive method
        private static string CreateJsonPayload(IDictionary<string, object> data, HttpContextBase context)
        {
            var sb = new StringBuilder("{");

            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    try
                    {
                        string dataString = Serializer.Serialize(item.Value);
                        sb.Append(string.Format("\"{0}\":{1},", item.Key, dataString));
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Problem serializing " + item.GetType().FullName, ex);

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
                requestMetadata.Add("runningVersion",
                                    decimal.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString(2),
                                                  NumberFormatInfo.InvariantInfo));

                //plugin specific metadata);))
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

            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}