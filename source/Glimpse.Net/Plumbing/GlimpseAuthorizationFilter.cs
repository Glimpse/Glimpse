using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseAuthorizationFilter : GlimpseFilter, IAuthorizationFilter
    {
        public IAuthorizationFilter AuthorizationFilter { get; set; }

        public GlimpseAuthorizationFilter(IAuthorizationFilter authorizationFilter)
        {
            AuthorizationFilter = authorizationFilter;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var watch = new Stopwatch();
            watch.Start();

            AuthorizationFilter.OnAuthorization(filterContext);

            watch.Stop();

            LogCall(Guid, watch.Elapsed);
        }
    }
}
