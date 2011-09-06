using System;
using System.ComponentModel.Composition;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class ContentTypeValidator:IGlimpseValidator
    {
        GlimpseConfiguration Configuration { get; set; }
        
        [ImportingConstructor]
        public ContentTypeValidator(GlimpseConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            Configuration = configuration;
        }

        public bool IsValid(HttpContextBase context, LifecycleEvent lifecycleEvent)
        {
            if (lifecycleEvent == LifecycleEvent.BeginRequest)
                return true;

            return Configuration.ContentTypes.Contains(context.Response.ContentType);
        }
    }
}
