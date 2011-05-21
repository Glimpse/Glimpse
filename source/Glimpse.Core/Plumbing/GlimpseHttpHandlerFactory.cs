using System;
using System.Web;

namespace Glimpse.WebForms.Plumbing
{
    internal class GlimpseHttpHandlerFactory:IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            throw new NotImplementedException();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
