using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class GlimpseModeValidator:IGlimpseValidator{
        public bool IsValid(HttpApplication application, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent)
        {
            //TODO: Test to see if thi sis needed
            if (lifecycleEvent == LifecycleEvent.BeginRequest)
                return true;

            return application.GetGlimpseMode() != GlimpseMode.Off;
        }
    }
}