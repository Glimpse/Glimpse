using System.IO;
using System.Reflection;
using System.Web;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.WebForms.Handler
{
    [GlimpseHandler]
    public class Javascript : IGlimpseHandler
    {
        public string ResourceName
        {
            get { return "glimpseClient.js"; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.WebForms.glimpseClient.js"))
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
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}