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
        internal static readonly string RunningVersion;
        internal static GlimpseRequestValidator RequestValidator { get; set; }
        private static IGlimpseSanitizer Sanitizer { get; set; }//TODO: new up via config

        [Export] public static IGlimpseFactory Factory { get; set; }
        [Export] public static GlimpseSerializer Serializer { get; set; }
        [Export] public static GlimpseConfiguration Configuration { get; set; }
        [Export] public static IGlimpseMetadataStore MetadataStore { get; set; }

        private static IGlimpseLogger Logger { get; set; }

        internal static IEnumerable<IGlimpseHandler> Handlers { get; set; }
        internal static IEnumerable<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        static Module()
        {
            RunningVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();

            Factory = new GlimpseFactory(Configuration);
            Logger = Factory.CreateLogger();

            Logger.Info(Configuration);

            RequestValidator = new GlimpseRequestValidator(Configuration, Enumerable.Empty<IGlimpseValidator>(), Factory);

            Sanitizer = new CSharpSanitizer();

            Serializer = new GlimpseSerializer(Factory);

            Handlers = Enumerable.Empty<IGlimpseHandler>();

            Plugins = Enumerable.Empty<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();

            Logger.Info("Glimpse Module constructed");
        }

        public void Init(HttpApplication application)
        {
            if (!Configuration.Enabled)
            {
                Logger.Info("Glimpse is not enabled and will not run. Change via the enable attribute on the glimpse element in web.config");
                return; //Do nothing if Glimpse is off, events are not wired up
            }

            //TODO: MetadataStore should be set via a configurable setting in web.config to allow for other backing stores
            MetadataStore = new InProcStackMetadataStore(Configuration, new HttpApplicationStateWrapper(application.Application));

            if (Plugins.Count() == 0)
            {
                lock (Plugins)
                {
                    if (Plugins.Count() == 0)
                    {
                        var contextBase = new HttpContextWrapper(application.Context);

                        ComposePlugins(); //Have MEF satisfy our needs

                        //Allow plugin's registered for Initialization to setup
                        foreach (var plugin in Plugins.Where(plugin => plugin.Metadata.ShouldSetupInInit))
                        {
                            Logger.Info("Calling SetupInit() on " + plugin.Value.GetType().FullName);
                            plugin.Value.SetupInit();
                        }
                    }
                }
            }

            application.BeginRequest += OnBeginRequest; //1
            application.PostMapRequestHandler += OnPostMapRequestHandler;//8
            application.PostRequestHandlerExecute += OnPostRequestHandlerExecute;//12
            application.PostReleaseRequestState += OnPostReleaseRequestState;//14
            application.EndRequest += OnEndRequest;//19
            application.PreSendRequestHeaders += OnPreSendRequestHeaders;//20

            Logger.Info("Glimpse Module Init Complete");
        }

        #region Event Handlers

        private static void OnBeginRequest(object sender, EventArgs e)//1
        {
            try
            {
                var httpApplication = sender as HttpApplication;

                if (httpApplication != null)
                    BeginRequest(new HttpContextWrapper(httpApplication.Context));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception during BeginRequest", exception);
            }
        }

        private static void OnPostMapRequestHandler(object sender, EventArgs e)//8
        {
            try
            {
                var httpApplication = sender as HttpApplication;

                if (httpApplication != null)
                    PostMapRequestHandler(new HttpContextWrapper(httpApplication.Context));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception during PostMapRequestHandler", exception);
            }
        }

        private static void OnPostRequestHandlerExecute(object sender, EventArgs e)//12
        {
            try
            {
                var httpApplication = sender as HttpApplication;

                if (httpApplication != null)
                    PostRequestHandlerExecute(new HttpContextWrapper(httpApplication.Context));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception during PostRequestHandlerExecute", exception);
            }
        }

        private static void OnPostReleaseRequestState(object sender, EventArgs e)//14
        {
            try
            {
                var httpApplication = sender as HttpApplication;

                if (httpApplication != null)
                    PostReleaseRequestState(new HttpContextWrapper(httpApplication.Context));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception during PostReleaseRequestState", exception);
            }
        }

        private static void OnEndRequest(object sender, EventArgs e)//19
        {
            try
            {
                var httpApplication = sender as HttpApplication;

                if (httpApplication != null)
                    EndRequest(new HttpContextWrapper(httpApplication.Context));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception during EndRequest", exception);
            }
        }

        private static void OnPreSendRequestHeaders(object sender, EventArgs e)//20
        {
            try
            {
                var httpApplication = sender as HttpApplication;

                if (httpApplication != null)
                    PreSendRequestHeaders(new HttpContextWrapper(httpApplication.Context));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception during PreSendRequestHeaders", exception);
            }
        }

        #endregion Event Handlers

        private static void BeginRequest(HttpContextBase context)//1
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.BeginRequest)) return;

            context.InitGlimpseContext();

            Logger.Info("BeginRequest handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path+")");
        }

        private static void PostMapRequestHandler(HttpContextBase context)//8
        {
            //temporary measure to help users move from glimpse/config to glimpse.asx
            if (context.Request.Path.ToLower().Contains(@"glimpse/config"))
            {
                Logger.Info("Noticed request for glimpse/config, redirecting");
                context.Response.RedirectPermanent(context.GlimpseResourcePath(null)+"?redirect=1", true);
            }
        }

        private static void PostRequestHandlerExecute(HttpContextBase context)//12
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PostRequestHandlerExecute)) return;

            ProcessData(context, true); //Run all plugins that DO need access to Session

            Logger.Info("PostRequestHandlerExecute handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
        }

        private static void PostReleaseRequestState(HttpContextBase context)//14
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PostReleaseRequestState)) return;

            if (!context.IsAjax())
                context.Response.Filter = new GlimpseResponseFilter(context.Response.Filter, context);

            Logger.Info("PostReleaseRequestState handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
        }

        private static void EndRequest(HttpContextBase context)//19
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.EndRequest)) return;

            ProcessData(context, false); //Run all plugins that DO NOT need access to Session

            var requestId = context.GetGlimpseRequestId().ToString();

            var jsonPayload = GenerateGlimpseOutput(context);
            Logger.Info("Glimpse output generated for requestId " + requestId + " (" + context.Request.Path + ")");

            MetadataStore.Persist(context.GetRequestMetadata(jsonPayload));
            Logger.Info("RequestId " + requestId + " (" + context.Request.Path + ")" + " persisted");

            Logger.Info("EndRequest handling complete for requestId " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
        }

        private static void PreSendRequestHeaders(HttpContextBase context)//20
        {
            if (!RequestValidator.IsValid(context, LifecycleEvent.PreSendRequestHeaders)) return;

            var requestId = context.GetGlimpseRequestId().ToString();

            context.Response.AddHeader(GlimpseConstants.HttpHeader, requestId);
        }

        #region Private Methods
        public void Dispose(){}

        private void ComposePlugins()
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

            return json;
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
                                    decimal.Parse(RunningVersion, NumberFormatInfo.InvariantInfo));

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
        #endregion Private Methods
    }
}