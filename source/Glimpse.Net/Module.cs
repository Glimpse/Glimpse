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
using Glimpse.Net.Converter;
using Glimpse.Net.Mvc;
using Glimpse.Net.Sanitizer;
using Glimpse.Protocol;

namespace Glimpse.Net
{
    public class Module : IHttpModule
    {
        private GlimpseConfiguration Configuration { get; set; }
        private CompositionContainer Container { get; set; }
        private IDictionary<string, object> Data { get; set; }
        private bool GlimpseRequest { get; set; }
        [ImportMany] private IList<IGlimpseConverter> JsConverters { get; set; }
        private JavaScriptSerializer JsSerializer { get; set; }
        private GlimpseMode Mode { get; set; }
        [ImportMany] private IList<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }
        [Import] private IGlimpseSanitizer Sanitizer { get; set; }
        private bool ValidIp { get; set; }

        public Module()
        {
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();
            JsConverters = new List<IGlimpseConverter>();
            JsSerializer = new JavaScriptSerializer();
            Plugins = new List<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();
        }

        /// <summary>
        /// Init method runs only when the server is starting.
        /// This method is use to configure Glimpse once.
        /// This methos will be recalled on the next request if web.config is changed.
        /// </summary>
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

        /// <summary>
        /// Runs at the begining of each requests, initializing Data (eventual return value) and parsing request params
        /// </summary>
        private void BeginRequest(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            GlimpseRequest = false;

            SetMode(httpApplication); //Read cookie and persist value
            SetValidIp(httpApplication); //Check IP adress once per request and persist

            if (GlimpseRequest = ProcessGlimpseRequest(httpApplication)) return;

            Data = new Dictionary<string, object>(); //Init the return data object
        }

        /// <summary>
        /// This method is only needed because its event is called before the session object is destroyed. Same as EndRequest
        /// </summary>
        private void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            if (InvalidRequest(httpApplication)) return; //ensure Glimpse should run

            ProcessData(httpApplication, true); //Run all plugins that DO need access to Session
        }

        /// <summary>
        /// Run all plugins that do nor require session access
        /// </summary>
        private void EndRequest(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            if (InvalidRequest(httpApplication)) return; //ensure Glimpse should run

            ProcessData(httpApplication, false); //Run all plugins that DO NOT need access to Session
        }

        /// <summary>
        /// Convert Data payload into JSON and attach to response
        /// </summary>
        private void PreSendRequestHeaders(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            if (InvalidRequest(httpApplication)) return;

            var json = JsSerializer.Serialize(Data); //serialize data to Json
            json = Sanitizer.Sanitize(json);

            Persist(json, httpApplication);

            //if ajax request, render glimpse data to headers
            if (new HttpRequestWrapper(httpApplication.Request).IsAjaxRequest()) //Wrapped so we can borrow MVC's IsAjax
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
            Container.ComposeParts(this);

            //wireup converters into serializer
            JsSerializer.RegisterConverters(JsConverters);
        }

        public void Dispose()
        {
            if (Container != null)
                Container.Dispose();
        }

        private string GetClientName(HttpApplication ctx)
        {
            var cookie = ctx.Request.Cookies[GlimpseConstants.CookieClientNameKey];
            return cookie != null ? cookie.Value : "";
        }

        private bool InvalidRequest(HttpApplication httpApplication)
        {
            var contentType = httpApplication.Response.ContentType;

            var validContentType = Configuration.ContentTypes.Contains(contentType);

            var result = (Mode == GlimpseMode.Off || !ValidIp || !validContentType || GlimpseRequest);

            return result;
        }

        private static void JsonResponse(HttpApplication httpApplication, string data)
        {
            var response = httpApplication.Response;
            response.Write(data);
            response.AddHeader("Content-Type", "application/json");
            httpApplication.CompleteRequest();
        }

        private void Persist(string json, HttpApplication ctx)
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
                                                        RequestId = Guid.NewGuid()
                                                    });
        }

        private void ProcessData(HttpApplication httpApplication, bool sessionRequired)
        {
            foreach (var plugin in Plugins)
            {
                if (plugin.Metadata.SessionRequired == sessionRequired)
                {
                    var p = plugin.Value;
                    var pluginData = p.GetData(httpApplication);
                    if (pluginData != null) Data.Add(p.Name, pluginData);
                }
            }
        }

        private bool ProcessGlimpseRequest(HttpApplication httpApplication)
        {
            //render config page
            if (httpApplication.Request.Path.StartsWith("/Glimpse/Config"))
            {
                var response = httpApplication.Response;

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
                response.Write(string.Format("</ol></li></ul><h1>Your Settings:</h1><ol><li>IP = {0}</li><li>GlimpseMode = <input type='checkbox' id='gChk' onclick='toggleCookie();'{2}/> <label for='gChk' id='glimpseMode'>{1}</lable></li></ol></body></html>", httpApplication.Request.ServerVariables["REMOTE_ADDR"], Mode, Mode==GlimpseMode.On ? " checked" : ""));

                httpApplication.CompleteRequest();
                return true;
            }

            //render history json
            if (httpApplication.Request.Path.StartsWith("/Glimpse/History"))
            {
                if (InvalidRequest(httpApplication))
                {
                    var data = JsSerializer.Serialize(new {Error = true, Message="You are not configured to access history."});
                    JsonResponse(httpApplication, data);
                    return true;
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
                    return true;
                }
                else
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "No history avalible." });
                    JsonResponse(httpApplication, data);
                    return true;
                }
            }

            if (httpApplication.Request.Path.StartsWith("/Glimpse/Clients"))
            {
                if (InvalidRequest(httpApplication))
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "You are not configured to access history." });
                    JsonResponse(httpApplication, data);
                    return true;
                }

                var queue = httpApplication.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
                if (queue != null)
                {
                    var filteredQueue = from request in queue
                                        group request by request.ClientName
                                        into clients select new {Client = clients.Key, RequestCount = clients.Count()};

                    var data = JsSerializer.Serialize(filteredQueue);
                    JsonResponse(httpApplication, data);
                    return true;
                }
                else
                {
                    var data = JsSerializer.Serialize(new { Error = true, Message = "No history avalible." });
                    JsonResponse(httpApplication, data);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Figure out if requestor is asking for glimpse to participate in this request based on cookie.
        /// No cookie is the same as cookie with off value
        /// </summary>
        private void SetMode(HttpApplication application)
        {
            var result = GlimpseMode.Off; //off by default
            var cookie = application.Request.Cookies[GlimpseConstants.CookieModeKey];

            if (cookie == null) //if the cookie does not exist, set the mode as off
            {
                Mode = result;
                return;
            }

            //if cookie exists, try to parse it out to a valid value.  If the value is not valid, the result will be GlimpseMode.Off
            GlimpseMode.TryParse(cookie.Value, true, out result);

            Mode = result;
        }

        /// <summary>
        /// Figures out if request is coming from a valid IP
        /// </summary>
        private void SetValidIp(HttpApplication httpApplication)
        {
            ValidIp = Configuration.IpAddresses.Contains(httpApplication.Request.ServerVariables["REMOTE_ADDR"]);
        }
    }
}