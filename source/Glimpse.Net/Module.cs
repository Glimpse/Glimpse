using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;
using Glimpse.Net.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net
{
    public class Module : IHttpModule
    {
        private static GlimpseConfiguration Configuration { get; set; }
        private static CompositionContainer Container { get; set; }
        [ImportMany] private IList<IGlimpseConverter> JsConverters { get; set; }
        private static JavaScriptSerializer JsSerializer { get; set; }
        [ImportMany] private IList<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }
        [Import] private IGlimpseSanitizer Sanitizer { get; set; }

        public Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();
            JsConverters = new List<IGlimpseConverter>();
            JsSerializer = new JavaScriptSerializer();
            Plugins = new List<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();
        }

        public void Init(HttpApplication context)
        {
            if (Configuration.On == false) return; //Do nothing if Glimpse is off, events are not wired up

            //TODO: MEF Plugin point to do something once as setup
            GlobalFilters.Filters.Add(new GlimpseFilterAttribute(), int.MinValue);

            var traceListeners = Trace.Listeners;
            if (!traceListeners.OfType<GlimpseTraceListener>().Any())
                traceListeners.Add(new GlimpseTraceListener()); //Add trace listener if it isn't already configured
            //TODO: END MEF Plugin point to do something once as setup

            ComposePlugins(); //Have MEF satisfy our needs

            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
            context.PostRequestHandlerExecute += PostRequestHandlerExecute;
            context.PreSendRequestHeaders += PreSendRequestHeaders;
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, false, false)) return;

            if (httpApplication.IsGlimpseRequest())
            {
                GlimpseResponse(httpApplication);
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

        private void PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication httpApplication;
            if (!sender.IsValidRequest(out httpApplication, Configuration, true)) return;

            IDictionary<string, object> data;
            if (!httpApplication.TryGetData(out data)) return;

            var json = JsSerializer.Serialize(data); //serialize data to Json
            json = Sanitizer.Sanitize(json);

            Persist(json, httpApplication);

            //if ajax request, render glimpse data to headers
            if (httpApplication.IsAjax())
            {
                httpApplication.Response.AddHeader(GlimpseConstants.HttpHeader, json);
            }
            else
            {
                var html = string.Format(@"<script type='text/javascript' id='glimpseData'>var glimpse = {0};</script>", json);
                httpApplication.Response.Write(html);
            }
        }

        private void ComposePlugins()
        {
            var aggregateCatalog = new AggregateCatalog();
            //var typeCatlog = new TypeCatalog(typeof (Plugin.Asp.Environment));
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var directoryCatalog = new DirectoryCatalog(@"\");

            //aggregateCatalog.Catalogs.Add(typeCatlog);
            aggregateCatalog.Catalogs.Add(assemblyCatalog);
            aggregateCatalog.Catalogs.Add(directoryCatalog);

            Container = new CompositionContainer(aggregateCatalog);
            Container.ComposeParts(this);//TODO, is this needed? can I use typeof?

            //wireup converters into serializer
            JsSerializer.RegisterConverters(JsConverters);
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

            if (queue == null) store[GlimpseConstants.JsonQueue] = queue = new Queue<GlimpseRequestMetadata>(Configuration.SaveRequestCount);

            if (queue.Count == Configuration.SaveRequestCount) queue.Dequeue();

            var browser = ctx.Request.Browser;
            queue.Enqueue(new GlimpseRequestMetadata{
                                                        Browser = string.Format("{0} {1}", browser.Browser, browser.Version),
                                                        ClientName = GetClientName(ctx),
                                                        Json = json,
                                                        RequestTime = DateTime.Now.ToLongTimeString(),
                                                        RequestId = Guid.NewGuid(),
                                                        IsAjax = new HttpRequestWrapper(ctx.Request).IsAjaxRequest().ToString()
                                                    });
        }

        private static string GetClientName(HttpApplication ctx)
        {
            var cookie = ctx.Request.Cookies[GlimpseConstants.CookieClientNameKey];
            return cookie != null ? cookie.Value : "";
        }

        private void ProcessData(HttpApplication httpApplication, bool sessionRequired)
        {
            IDictionary<string, object > data;
            if (!httpApplication.TryGetData(out data)) return;
            
            foreach (var plugin in Plugins)
            {
                if (plugin.Metadata.SessionRequired == sessionRequired)
                {
                    var p = plugin.Value;
                    var pluginData = p.GetData(httpApplication);
                    if (pluginData != null) data.Add(p.Name, pluginData);
                }
            }
            //TODO: do I need to reassign to the store?
        }

        private static void GlimpseResponse(HttpApplication httpApplication)
        {
            //TODO: CLEAN ME!
            var path = httpApplication.Request.Path;
            var response = httpApplication.Response;

            //render config page
            if (path.StartsWith("/Glimpse/Config"))
            {
                var mode = httpApplication.GetGlimpseMode();

                response.Write(string.Format("<html><head><title>Glimpse Config</title><script>function toggleCookie(){{var mode = document.getElementById('glimpseMode'); if (mode.innerHTML==='On'){{mode.innerHTML='Off';document.cookie='glimpseMode=Off; path=/;'}}else{{mode.innerHTML='On';document.cookie='glimpseMode=On; path=/;'}}}}</script><head><body><h1>Glimpse Config Settings:</h1><ul><li>On = {0}</li><li>Allowed IP's = <ol>", Configuration.On));
                foreach (IpAddress ipAddress in Configuration.IpAddresses)
                {
                    response.Write(string.Format("<li>{0}</li>", ipAddress.Address));
                }
                response.Write("</ol></li><li>Allowed ContentType's = <ol>");
                foreach (ContentType contentType in Configuration.ContentTypes)
                {
                    response.Write(string.Format("<li>{0}</li>", contentType.Content));
                }
                response.Write(string.Format("</ol></li></ul><h1>Your Settings:</h1><ol><li>IP = {0}</li><li>GlimpseMode = <input type='checkbox' id='gChk' onclick='toggleCookie();'{2}/> <label for='gChk' id='glimpseMode'>{1}</lable></li></ol></body></html>", httpApplication.Request.ServerVariables["REMOTE_ADDR"], mode, mode==GlimpseMode.On ? " checked" : ""));

                httpApplication.CompleteRequest();
                return;
            }

            //render history json
            if (path.StartsWith("/Glimpse/History"))
            {
                if (!httpApplication.IsValidRequest(Configuration, false, checkPath:false))
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "You are not configured to access history." });
                    JsonResponse(httpApplication, data);
                    return;
                }

                var queue = httpApplication.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
                if (queue != null)
                {
                    var clientName = httpApplication.Request.QueryString[GlimpseConstants.ClientName];
                    string data;
                    
                    if (string.IsNullOrEmpty(clientName))
                        data = JsSerializer.Serialize(queue);
                    else
                    {
                        var filteredQueue = from request in queue where request.ClientName.Equals(clientName) select request;
                        data = JsSerializer.Serialize(filteredQueue);
                    }

                    JsonResponse(httpApplication, data);
                    return;
                }
                else
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "No history avalible." });
                    JsonResponse(httpApplication, data);
                    return;
                }
            }

            if (path.StartsWith("/Glimpse/Clients"))
            {
                if (!httpApplication.IsValidRequest(Configuration, false, checkPath:false))
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "You are not configured to access history." });
                    JsonResponse(httpApplication, data);
                    return;
                }

                var queue = httpApplication.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
                if (queue != null)
                {
                    var filteredQueue = from request in queue
                                        group request by request.ClientName
                                        into clients select new {Client = clients.Key, RequestCount = clients.Count()};

                    var data = JsSerializer.Serialize(filteredQueue);
                    JsonResponse(httpApplication, data);
                    return;
                }
                else
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "No history avalible." });
                    JsonResponse(httpApplication, data);
                    return;
                }
            }
        }

        private static void JsonResponse(HttpApplication httpApplication, string data)
        {
            var response = httpApplication.Response;
            response.Write(data);
            response.AddHeader("Content-Type", "application/json");
            httpApplication.CompleteRequest();
        }
    }
}