using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
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
        [ImportMany]
        private IList<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        private IDictionary<string, IDictionary<string, string>> Data { get; set; }
        private GlimpseConfiguration Configuration { get; set; }
        private bool ValidIp { get; set; }
        private GlimpseMode Mode { get; set; }
        private CompositionContainer Container { get; set; }

        public Module()
        {
            Plugins = new List<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>>();
            Configuration = ConfigurationManager.GetSection("glimpse") as GlimpseConfiguration ?? new GlimpseConfiguration();
        }

        public void Init(HttpApplication context)
        {
            if (Configuration.On == false) return;

            //TODO: MEF Plugin point to do something once as setup
            GlobalFilters.Filters.Add(new GlimpseFilterAttribute(), int.MaxValue);

            ComposePlugins();

            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
            context.PostRequestHandlerExecute += PostRequestHandlerExecute;
            context.PreSendRequestHeaders += PreSendRequestHeaders;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            SetMode(httpApplication);
            SetValidIp(httpApplication);
            
            //TODO: MEF Plugin point to do something at the begining of every request
            Data = new Dictionary<string, IDictionary<string, string>>();
        }

        private void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            if (InvalidRequest(httpApplication)) return;

            ProcessData(httpApplication, true);
        }

        private void EndRequest(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            if (InvalidRequest(httpApplication)) return;

            ProcessData(httpApplication, false);
        }

        private void PreSendRequestHeaders(object sender, EventArgs e)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication == null) return;

            if (InvalidRequest(httpApplication)) return;

            var serializer = new JavaScriptSerializer();
            var output = serializer.Serialize(Data);

            switch (Mode)
            {
                case GlimpseMode.Header:
                    httpApplication.Response.AddHeader(GlimpseConstants.HttpHeader, output);
                    return;
                case GlimpseMode.Body:
                    var html = string.Format(@"<script type='text/javascript'>var glimpse = {0};</script>", output);
                    httpApplication.Response.Write(html);
                    return;
            }
        }

        public void Dispose()
        {
            if (Container != null)
                Container.Dispose();
        }

        private void ComposePlugins()
        {
            var aggregateCatalog = new AggregateCatalog();
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var directoryCatalog = new DirectoryCatalog(@"\");

            aggregateCatalog.Catalogs.Add(assemblyCatalog);
            aggregateCatalog.Catalogs.Add(directoryCatalog);

            Container = new CompositionContainer(aggregateCatalog);
            Container.ComposeParts(this);
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

        private void SetMode(HttpApplication application)
        {
            var result = GlimpseMode.Off;
            var cookie = application.Request.Cookies[GlimpseConstants.CookieKey];

            if (cookie == null)
            {
                Mode = result;
                return;
            }

            GlimpseMode.TryParse(cookie.Value, true, out result);

            Mode = result;
        }

        private void SetValidIp(HttpApplication httpApplication)
        {
            ValidIp = Configuration.IpAddresses.Contains(httpApplication.Request.ServerVariables["REMOTE_ADDR"]);
        }

        private bool InvalidRequest(HttpApplication httpApplication)
        {
            var contentType = httpApplication.Response.ContentType;

            var validContentType = Configuration.ContentTypes.Contains(contentType);

            return (Mode == GlimpseMode.Off || !ValidIp || !validContentType);
        }
    }
}