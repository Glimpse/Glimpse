using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Extensions;
using Glimpse.Net.Warning;

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
            var warnings = HttpContext.Current.GetWarnings();

            //handle static registered binders
            var binders = System.Web.Mvc.ModelBinders.Binders;
            var keys = binders.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var type = keys[i];
                var binder = binders[type];

                if (binder is DefaultModelBinder)
                    if (binder.CanSupportDynamicProxy())
                    {
                        binders[type] = binder.CreateDynamicProxy();
                        continue;
                    }

                warnings.Add(new NotADefaultModelBinderWarning(binder));
                binders[type] = binder.Wrap();
            }

            //handle default binder
            var defaultBinder = System.Web.Mvc.ModelBinders.Binders.DefaultBinder;
            if (defaultBinder is DefaultModelBinder)
                if (defaultBinder.CanSupportDynamicProxy())
                {
                    System.Web.Mvc.ModelBinders.Binders.DefaultBinder = defaultBinder.CreateDynamicProxy();
                    return;
                }


            warnings.Add(new NotADefaultModelBinderWarning(defaultBinder));
            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = defaultBinder.Wrap();
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

        public static void ModelBinderProviders()
        {
            var binderProviders = System.Web.Mvc.ModelBinderProviders.BinderProviders;

            for (int i = 0; i < binderProviders.Count; i++)
            {
                binderProviders[i] = new GlimpseBinderProvider(binderProviders[i]);
            }
        }
    }
}