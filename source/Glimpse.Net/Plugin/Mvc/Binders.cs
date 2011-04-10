using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Plumbing;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Binders:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Binding"; }
        }

        public object GetData(HttpApplication application)
        {
            return "Binding plugin still in development.  Check Trace tab for more info.";
        }

        public void SetupInit(HttpApplication application)
        {
            //Wrap all binders
            var binders = ModelBinders.Binders;
            var keys = binders.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var type = keys[i];

                if (!(binders[type] is GlimpseModelBinder))
                    binders[type] = new GlimpseModelBinder(binders[type]);
            }
            if (!(ModelBinders.Binders.DefaultBinder is GlimpseDefaultModelBinder))
                ModelBinders.Binders.DefaultBinder = new GlimpseDefaultModelBinder();

            //Wrap all providers/provider factories
            var factories = ValueProviderFactories.Factories;

            for (int i = 0; i < factories.Count; i++)
            {
                if (!(factories[i] is GlimpseValueProviderFactory))
                    factories[i] = new GlimpseValueProviderFactory(factories[i]);
            }
        }
    }
}
