using System.IO;
using System.Reflection;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handler
{
    [GlimpseHandler]
    public class Javascript : HandlerBase
    {
        public override string ResourceName
        {
            get { return "glimpseClient"; }
        }

        public override void Process(HttpContextBase context)
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

        public override bool IsReusable
        {
            get { return true; }
        }
    }
}