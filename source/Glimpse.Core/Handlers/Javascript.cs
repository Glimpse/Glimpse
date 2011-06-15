using System.IO;
using System.Reflection;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class Javascript : IGlimpseHandler
    {
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
        }
    }
}