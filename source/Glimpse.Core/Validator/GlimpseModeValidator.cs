using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class GlimpseModeValidator:IGlimpseValidator{
        public bool IsValid(HttpContextBase context, LifecycleEvent lifecycleEvent)
        {
            if (lifecycleEvent == LifecycleEvent.BeginRequest || lifecycleEvent == LifecycleEvent.Handler)
                return true;

            return context.GetGlimpseMode() != GlimpseMode.Off;
        }
    }
}