using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Validator
{
    internal class GlimpseRequestValidator
    {
        private GlimpseConfiguration Configuration { get; set; }
        [ImportMany] public IEnumerable<IGlimpseValidator> Validators { get; set; }

        public GlimpseRequestValidator(GlimpseConfiguration configuration, IEnumerable<IGlimpseValidator> validators)
        {
            Configuration = configuration;
            Validators = validators;
        }

        public bool IsValid(HttpContextBase context, LifecycleEvent lifecycleEvent)
        {
            if (context == null) return false;

            foreach (var validator in Validators)
            {
                if (!validator.IsValid(context, Configuration, lifecycleEvent))
                    return false;
            }

            return true;
        }
    }
}