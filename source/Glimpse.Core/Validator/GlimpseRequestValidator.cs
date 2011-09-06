using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Validator
{
    internal class GlimpseRequestValidator
    {
        [ImportMany] public IEnumerable<IGlimpseValidator> Validators { get; set; }
        public IGlimpseLogger Logger { get; set; }

        public GlimpseRequestValidator(IEnumerable<IGlimpseValidator> validators, IGlimpseFactory factory)
        {
            Validators = validators;
            Logger = factory.CreateLogger();
        }

        public bool IsValid(HttpContextBase context, LifecycleEvent lifecycleEvent)
        {
            if (context == null) return false;

            foreach (var validator in Validators)
            {
                if (!validator.IsValid(context, lifecycleEvent))
                {
                    Logger.Warn(validator.GetType().FullName + " invalided request (it will now be ignored) with id " + context.GetGlimpseRequestId() + " (" + context.Request.Path + ")");
                    return false;
                }
            }

            return true;
        }
    }
}