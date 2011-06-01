using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class GlimpseModeValidator:IGlimpseValidator{
        public bool IsValid(HttpContextBase context, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent)
        {
            //TODO: Test to see if this is needed
            if (lifecycleEvent == LifecycleEvent.BeginRequest)
                return true;

            return context.GetGlimpseMode() != GlimpseMode.Off;
        }
    }
}