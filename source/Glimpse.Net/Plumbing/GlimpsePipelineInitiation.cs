using System.Linq;
using System.Web.Mvc;
using Glimpse.Net.Extensions;

namespace Glimpse.Net.Plumbing
{
    public static class GlimpsePipelineInitiation
    {
        public static void ControllerFactory()
        {
            var controllerBuilder = ControllerBuilder.Current.GetControllerFactory();
            if (controllerBuilder != null) ControllerBuilder.Current.SetControllerFactory(controllerBuilder.Wrap());
        }

        public static void DependencyResolver()
        {
            var dependencyResolver = System.Web.Mvc.DependencyResolver.Current;
            if (dependencyResolver != null && !(dependencyResolver is GlimpseDependencyResolver))
                System.Web.Mvc.DependencyResolver.SetResolver(new GlimpseDependencyResolver(dependencyResolver));
        }

        public static void ModelBinders()
        {
            //TODO: Proxy DefaultModelBinder if I can
            var binders = System.Web.Mvc.ModelBinders.Binders;
            var keys = binders.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var type = keys[i];

                if (!(binders[type] is GlimpseModelBinder))
                    binders[type] = new GlimpseModelBinder(binders[type]);
            }

            if (!(System.Web.Mvc.ModelBinders.Binders.DefaultBinder is GlimpseDefaultModelBinder))
                System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new GlimpseDefaultModelBinder();
        }

        public static void ValueProviders()
        {
            //TODO: Need to proxy value providers?
            var factories = ValueProviderFactories.Factories;

            for (int i = 0; i < factories.Count; i++)
            {
                if (!(factories[i] is GlimpseValueProviderFactory))
                    factories[i] = new GlimpseValueProviderFactory(factories[i]);
            }
        }
    }
}
