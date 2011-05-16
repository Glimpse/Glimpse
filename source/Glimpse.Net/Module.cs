using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Web;
using Glimpse.Net.Responder;
using Glimpse.Net.Warning;
using Glimpse.WebForms.Configuration;
using Glimpse.WebForms.Extensibility;
using Glimpse.WebForms.Extensions;
using Glimpse.WebForms.Plumbing;

namespace Glimpse.Net
{
    public class Module : IHttpModule
    {
        private static GlimpseConfiguration Configuration { get; set; }
        private static BlacklistedSafeDirectoryCatalog DirectoryCatalog { get; set; }
        private static CompositionContainer Container { get; set; }
        private static GlimpseResponders Responders { get; set; }

        [ImportMany]
        internal static IEnumerable<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        static Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();
            Responders = new GlimpseResponders();
            Plugins = Enumerable.Empty<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();

            DirectoryCatalog = new BlacklistedSafeDirectoryCatalog("bin", Configuration.PluginBlacklist.TypeNames());

            Container = new CompositionContainer(DirectoryCatalog);
        }

        public void Init(HttpApplication context)
        {
            if (Configuration.On == false) return; //Do nothing if Glimpse is off, events are not wired up

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
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, false, false)) return;

            var responder = Responders.GetResponderFor(httpApplication);
            if (responder != null)
            {
                responder.Respond(httpApplication, Configuration);
                return;
            }

            httpApplication.InitGlimpseContext();
        }

        private static void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            ProcessData(httpApplication, true); //Run all plugins that DO need access to Session
        }

        private static void EndRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            ProcessData(httpApplication, false); //Run all plugins that DO NOT need access to Session
        }

        private static void PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            var requestId = Guid.NewGuid();

            var json = Responders.StandardResponse(httpApplication, requestId);

            Persist(json, httpApplication, requestId);
        }

        private void ComposePlugins(HttpApplication application)
        {
            var batch = new CompositionBatch();

            Container.ComposeParts(this, Responders);

            Container.Compose(batch);

            Plugins = Container.GetExports<IGlimpsePlugin, IGlimpsePluginRequirements>();

            var store = application.Context.GetWarnings();
            store.AddRange(DirectoryCatalog.Exceptions.Select(exception => new ExceptionWarning(exception)));

            //wireup converters into serializer
            Responders.RegisterConverters();
        }

        public void Dispose()
        {
/*
            if (Container != null)
                Container.Dispose();
*/
        }

        private static void Persist(string json, HttpApplication ctx, Guid requestId)
        {
            if (Configuration.SaveRequestCount <= 0) return;

            var store = ctx.Application;

            //clientName, longtime, url, 
            var queue = store[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;

            if (queue == null)
                store[GlimpseConstants.JsonQueue] =
                    queue = new Queue<GlimpseRequestMetadata>(Configuration.SaveRequestCount);

            if (queue.Count == Configuration.SaveRequestCount) queue.Dequeue();

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
    }
}