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
            if (!GlimpseRuntime.IsInitialized)
            {
                throw new HttpException(404, Resources.ProcessRequestMissingRuntime);
            }

            bool glimpseRequestContextHandleFound = false;

            if (context.Items.Contains(Constants.GlimpseRequestContextHandle))
            {
                var glimpseRequestContextHandle = (GlimpseRequestContextHandle)context.Items[Constants.GlimpseRequestContextHandle];
                if (glimpseRequestContextHandle != null)
                {
                    glimpseRequestContextHandleFound = true;

                    var queryString = context.Request.QueryString;
                    var resourceName = queryString[UriTemplateResourceEndpointConfiguration.DefaultResourceNameKey];

                    if (string.IsNullOrEmpty(resourceName))
                    {
                        GlimpseRuntime.Instance.ExecuteDefaultResource(glimpseRequestContextHandle);
                    }
                    else
                    {
                        GlimpseRuntime.Instance.ExecuteResource(glimpseRequestContextHandle, resourceName, new ResourceParameters(queryString.AllKeys.Where(key => key != null).ToDictionary(key => key, key => queryString[key])));
                    }
                }
            }

            if (!glimpseRequestContextHandleFound)
            {
                GlimpseRuntime.Instance.Configuration.Logger.Info("There is no Glimpse request context handle stored inside the httpContext.Items collection.");
            }
        }
    }
}