using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Glimpse.Net.Configuration;
using Glimpse.Net.Responder;
using Glimpse.Protocol;

namespace Glimpse.Net
{
    public class Module : IHttpModule
    {
        private static GlimpseConfiguration Configuration { get; set; }
        private static CompositionContainer Container { get; set; }
        private static GlimpseResponders Responders { get; set; }
        [ImportMany] private IList<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        public Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();
            Responders = new GlimpseResponders();
            Plugins = new List<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();
        }

        public void Init(HttpApplication context)
        {
            if (Configuration.On == false) return; //Do nothing if Glimpse is off, events are not wired up

            ComposePlugins(); //Have MEF satisfy our needs

            //Allow plugin's registered for Intialization to setup
            foreach (var plugin in Plugins.Where(plugin => plugin.Metadata.ShouldSetupInInit))
            {
                plugin.Value.SetupInit();
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

            httpApplication.SetData(new Dictionary<string, object>());
        }

        private void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            ProcessData(httpApplication, true); //Run all plugins that DO need access to Session
        }

        private void EndRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            ProcessData(httpApplication, false); //Run all plugins that DO NOT need access to Session
        }

        private static void PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            var json = Responders.StandardResponse(httpApplication);

            Persist(json, httpApplication);
        }

        private void ComposePlugins()
        {
            var aggregateCatalog = new AggregateCatalog();
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var directoryCatalog = new DirectoryCatalog(@"\");
            var configuredDirectoryCatalog = new DirectoryCatalog(Configuration.PluginPath);

            aggregateCatalog.Catalogs.Add(assemblyCatalog);
            aggregateCatalog.Catalogs.Add(directoryCatalog);
            aggregateCatalog.Catalogs.Add(configuredDirectoryCatalog);

            Container = new CompositionContainer(aggregateCatalog);
            Container.ComposeParts(this, Responders);

            //wireup converters into serializer
            Responders.RegisterConverters();
        }

        public void Dispose()
        {
            if (Container != null)
                Container.Dispose();
        }

        private static void Persist(string json, HttpApplication ctx)
        {
            if (Configuration.SaveRequestCount <= 0) return;

            var store = ctx.Application;
            Queue<GlimpseRequestMetadata> queue = null;

            //clientName, longtime, url, 
            queue = store[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;

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
                                  RequestId = Guid.NewGuid(),
                                  IsAjax = ctx.IsAjax().ToString()
                              });
        }

        private void ProcessData(HttpApplication httpApplication, bool sessionRequired)
        {
            IDictionary<string, object> data;
            if (!httpApplication.TryGetData(out data)) return;

            foreach (var plugin in Plugins.Where(p=>p.Metadata.SessionRequired == sessionRequired))
            {
                var p = plugin.Value;
                try
                {
                    var pluginData = p.GetData(httpApplication);
                    data.Add(p.Name, pluginData);
                }
                catch(Exception ex)
                {
                    data.Add(p.Name, ex.Message);
                }
            }
        }
    }
}