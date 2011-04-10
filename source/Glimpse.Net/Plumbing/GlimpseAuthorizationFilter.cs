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
            Debug.Write(string.Format("{0} for {1} controller which is child:{2}", "OnAuthorization", filterContext.Controller.GetType().Name, filterContext.IsChildAction));
            LogCall(Guid);

            AuthorizationFilter.OnAuthorization(filterContext);
        }
    }
}
