using System.Web;
using Glimpse.WebForms.Configuration;

namespace Glimpse.WebForms.Responder
{
    public abstract class GlimpseResponder
    {
        public abstract string ResourceName { get; }
        public abstract void Respond(HttpApplication application, GlimpseConfiguration config);
    }
}