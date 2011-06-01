using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Validator;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseValidator
    {
        bool IsValid(HttpContextBase context, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent);
    }
}