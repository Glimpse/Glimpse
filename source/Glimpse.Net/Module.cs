using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;
using Glimpse.Net.Converter;
using Glimpse.Net.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net
{
    public class Module : IHttpModule
    {
        [ImportMany]
        private IList<Lazy<IGlimpsePlugin, IGlimpsePluginRequirements>> Plugins { get; set; }

        private IDictionary<string, object> Data { get; set; }
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
            GlobalFilters.Filters.Add(new GlimpseFilterAttribute(), int.MinValue);
            
            if (!Trace.Listeners.OfType<GlimpseTraceListener>().Any())
            {
                Trace.Listeners.Add(new GlimpseTraceListener(context.Context.Items));
            }
            //TODO: END MEF Plugin point to do something once as setup

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
            Data = new Dictionary<string, object>();
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
            //TODO: MEFify this
            var converters = new List<JavaScriptConverter>
                                 {
                                     new HandleErrorAttributeConverter(),
                                     new OutputCacheAttributeConverter(),
                                     new RouteValueDictionaryConverter()
                                 };

            serializer.RegisterConverters(converters);
            var output = serializer.Serialize(Data);
            output = Clean(output);

            //if ajax request, render glimpse data to headers
            if (new HttpRequestWrapper(httpApplication.Request).IsAjaxRequest())
            {
                httpApplication.Response.AddHeader(GlimpseConstants.HttpHeader, output);
            }
            else
            {
                var html = string.Format(@"<script type='text/javascript'>var glimpse = {0};</script>", output);
                httpApplication.Response.Write(html);
            }
        }

        //TODO : Refactor into "CleaningProvider"
        private string Clean(string json)
        {
            json = Regex.Replace(json, @"(?<=`[0-9]+\[.+)\](?=""| )", @">"); //Replace '>' for generics
            json = Regex.Replace(json, @"`[0-9]\[", @"<"); //Replace '<' for generics
            json = Regex.Replace(json, @"(?<=System\.Nullable<.+)>(?= )", @"?"); //Add '?' for nullable types
            json = Regex.Replace(json, @"System.Nullable<", @""); //Add '?' for nullable types
            json = json.Replace("System.Boolean", "bool"); //Convert CLR type names to c# keywords
            json = json.Replace("System.Byte", "byte");
            json = json.Replace("System.SByte", "sbyte");
            json = json.Replace("System.Char", "char");
            json = json.Replace("System.Decimal", "decimal");
            json = json.Replace("System.Double", "double");
            json = json.Replace("System.Single", "float");
            json = json.Replace("System.Int32", "int");
            json = json.Replace("System.UInt32", "uint");
            json = json.Replace("System.Int64", "long");
            json = json.Replace("System.UInt64", "ulong");
            json = json.Replace("System.Object", "object");
            json = json.Replace("System.Int16", "short");
            json = json.Replace("System.UInt16", "ushort");
            json = json.Replace("System.String", "string");
            json = json.Replace("-2147483648", "\"int.MinValue\"");
            json = json.Replace("2147483647", "\"int.MaxValue\"");

            var matches = Regex.Matches(json, @"\\/Date\((?<ticks>(\d+))\)\\/");

            long ticks;
            var epoch = new DateTime(1970, 1, 1);

            foreach (Match match in matches)
            {
                if (long.TryParse(match.Groups["ticks"].Value, out ticks))
                {
                    var dateTime = epoch.AddMilliseconds(ticks).ToLocalTime();
                    json = json.Replace(match.Value, dateTime.ToString());
                }
            }


            return json;
        }

        public void Dispose()
        {
            if (Container != null)
                Container.Dispose();
        }

        private void ComposePlugins()
        {
            var aggregateCatalog = new AggregateCatalog();
            //var typeCatlog = new TypeCatalog(typeof (Plugin.Mvc.Routes));
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var directoryCatalog = new DirectoryCatalog(@"\");

            //aggregateCatalog.Catalogs.Add(typeCatlog);
            //aggregateCatalog.Catalogs.Add(new TypeCatalog(typeof(Plugin.Mvc.Filters)));
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