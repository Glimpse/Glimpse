using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class JavascriptClient : IGlimpseHandler
    {
        [Import]
        internal GlimpseConfiguration Configuration { get; set; }

        public string ResourceName
        {
            get { return "client.js"; }
        }
         
        public void ProcessRequest(HttpContextBase context)
        {
            var response = context.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.Core.glimpseClient.js"))
            {
                if (resourceStream != null)
                {
                    using (var reader = new StreamReader(resourceStream))
                    {
                        response.Write(reader.ReadToEnd());
                    }
                }
            }
            response.AddHeader("Content-Type", "application/x-javascript");
            if (!Configuration.CacheDisabled)
                response.ExpiresAbsolute = DateTime.Now.AddMonths(6);
        }
    }
}