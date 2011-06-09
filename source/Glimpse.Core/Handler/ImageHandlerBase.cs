using System.Reflection;
using System.Web;

namespace Glimpse.Core.Handler
{
    public abstract class ImageHandlerBase : HandlerBase
    {
        protected abstract string ContentType { get; }

        public override void Process(HttpContextBase context)
        {
            var response = context.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.Core." + ResourceName + ".png"))
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

        public override bool IsReusable
        {
            get { return true; }
        }
    }
}