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
            var glimpseRuntimeWrapper = context.Application.Get(Constants.RuntimeKey) as GlimpseRuntimeWrapper;

            if (glimpseRuntimeWrapper == null)
            {
                throw new HttpException(404, Resources.ProcessRequestMissingRuntime);
            }

            var queryString = context.Request.QueryString;

            var resourceName = queryString["n"];

            if (string.IsNullOrEmpty(resourceName))
            {
                glimpseRuntimeWrapper.ExecuteDefaultResource();
            }
            else
            {
                glimpseRuntimeWrapper.ExecuteResource(resourceName, new ResourceParameters(queryString.AllKeys.Where(key => key != null).ToDictionary(key => key, key => queryString[key])));
            }
        }
    }
}