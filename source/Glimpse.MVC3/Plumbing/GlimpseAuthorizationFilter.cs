using System;
using System.Diagnostics;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseAuthorizationFilter : GlimpseFilter, IAuthorizationFilter
    {
        public IAuthorizationFilter AuthorizationFilter { get; set; }
        public Guid Guid { get; set; }

        public GlimpseAuthorizationFilter(IAuthorizationFilter authorizationFilter)
        {
            AuthorizationFilter = authorizationFilter;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var metadata = LogCall(Guid);
            var watch = new Stopwatch();
            watch.Start();

            using (GlimpseTimer.Start("Executing IAuthorizationFilter " + AuthorizationFilter.GetType().Name, "MVC"))
            {
                AuthorizationFilter.OnAuthorization(filterContext);
            }

            watch.Stop();

            metadata.ExecutionTime = watch.Elapsed;
        }
    }
}
