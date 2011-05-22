using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Validator;

namespace Glimpse.Core.Extensibility
{
    public interface IGlimpseValidator
    {
        bool IsValid(HttpApplication application, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent);
    }
}