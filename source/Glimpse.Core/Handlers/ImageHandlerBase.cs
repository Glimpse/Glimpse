using System.Reflection;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handlers
{
    public abstract class ImageHandlerBase : IGlimpseHandler
    {
        protected abstract string ContentType { get; }
        public abstract string ResourceName { get;} 

        public void ProcessRequest(HttpContextBase context)
        {
            var response = context.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.Core." + ResourceName))
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

    }
}