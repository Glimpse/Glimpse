using System.Reflection;
using System.Web;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.WebForms.Handler
{
    public abstract class ImageHandlerBase : IGlimpseHandler
    {
        public abstract string ContentType { get; }
        public abstract string ResourceName { get; }

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.WebForms." + ResourceName))
            {
                if (resourceStream != null)
                {
                    var byteArray = new byte[resourceStream.Length];
                    resourceStream.Read(byteArray, 0, byteArray.Length);
                    response.OutputStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            response.AddHeader("Content-Type", ContentType);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}