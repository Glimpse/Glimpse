using System;
using System.Reflection;
using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handlers
{
    public abstract class ImageHandlerBase : IGlimpseHandler
    {
        protected abstract string ContentType { get; }
        protected abstract string EmbeddedResourceName { get; }
        public abstract string ResourceName { get;} 


        public void ProcessRequest(HttpContextBase context)
        {
            var response = context.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream(EmbeddedResourceName))
            {
                if (resourceStream != null)
                {
                    var byteArray = new byte[resourceStream.Length];
                    resourceStream.Read(byteArray, 0, byteArray.Length);
                    response.OutputStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            response.AddHeader("Content-Type", ContentType);
            response.ExpiresAbsolute = DateTime.Now.AddYears(1);
        }

    }
}