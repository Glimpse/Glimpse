using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class ModelBinders:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Model Binders"; }
        }

        public object GetData(HttpApplication application)
        {
            var binders = System.Web.Mvc.ModelBinders.Binders;
            var result = new Dictionary<string, object>();

            foreach (var binder in binders)
            {
                result.Add(binder.Key.Name, binder.Value.GetType().Name);
            }

            return result;
        }

        public void SetupInit(HttpApplication application)
        {
            //System.Web.Mvc.ModelBinders.Binders.
        }
    }
}
