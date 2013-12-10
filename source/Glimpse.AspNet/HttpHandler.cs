using System.Linq;
using System.Web;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        public void ProcessRequest(HttpContextBase context)
        {
            var runtime = GlimpseRuntime.Instance;

            if (runtime == null)
            {
                throw new HttpException(404, Resources.ProcessRequestMissingRuntime);
            }

            var queryString = context.Request.QueryString;

            var resourceName = queryString["n"];
            var frameworkProvider = new AspNetFrameworkProvider(context, runtime.Configuration.Logger);

            if (string.IsNullOrEmpty(resourceName))
            {
                runtime.ExecuteDefaultResource(frameworkProvider);
            }
            else
            {
                runtime.ExecuteResource(frameworkProvider, resourceName, new ResourceParameters(queryString.AllKeys.Where(key => key != null).ToDictionary(key => key, key => queryString[key])));
            }
        }
    }
}