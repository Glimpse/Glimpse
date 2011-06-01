using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class ResponseCodeValidator:IGlimpseValidator
    {
        public bool IsValid(HttpContextBase context, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent)
        {
            var validResponseCodes = new List<int> { 200, 301, 302 };
            return validResponseCodes.Contains(context.Response.StatusCode);
        }
    }
}
