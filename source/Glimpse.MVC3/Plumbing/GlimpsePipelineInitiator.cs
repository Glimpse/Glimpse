using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpsePipelineInitiator
    {
        public IGlimpseLogger Logger { get; set; }

        public GlimpsePipelineInitiator(IGlimpseLogger logger)
        {
            Logger = logger;
        }

        public void ControllerFactory()
        {
            var controllerBuilder = ControllerBuilder.Current.GetControllerFactory();
            if (controllerBuilder != null) ControllerBuilder.Current.SetControllerFactory(controllerBuilder.Wrap(Logger));
        }

        public void DependencyResolver()
        {
            var dependencyResolver = System.Web.Mvc.DependencyResolver.Current;
            if (dependencyResolver != null && !(dependencyResolver is GlimpseDependencyResolver))
                System.Web.Mvc.DependencyResolver.SetResolver(new GlimpseDependencyResolver(dependencyResolver, Logger));
        }

        public void ModelBinders()
        {
            //handle static registered binders
            var binders = System.Web.Mvc.ModelBinders.Binders;
            var keys = binders.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var type = keys[i];
                var binder = binders[type];

                if (binder is DefaultModelBinder)
                    if (binder.CanSupportDynamicProxy(Logger))
                    {
                        binders[type] = binder.CreateDynamicProxy(Logger);
                        continue;
                    }

                Logger.Warn(binder.GetType() + " is not a System.Web.Mvc.DefaultModelBinder.");

                binders[type] = binder.Wrap();
            }

            //handle default binder
            var defaultBinder = System.Web.Mvc.ModelBinders.Binders.DefaultBinder;
            if (defaultBinder is DefaultModelBinder)
                if (defaultBinder.CanSupportDynamicProxy(Logger))
                {
                    System.Web.Mvc.ModelBinders.Binders.DefaultBinder = defaultBinder.CreateDynamicProxy(Logger);
                    return;
                }

            Logger.Warn(defaultBinder.GetType() + " is not a System.Web.Mvc.DefaultModelBinder.");

            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = defaultBinder.Wrap();
        }

        public void ValueProviders()
        {
            //TODO: Need to proxy value providers?
            var factories = ValueProviderFactories.Factories;

            for (int i = 0; i < factories.Count; i++)
            {
                if (!(factories[i] is GlimpseValueProviderFactory))
                    factories[i] = new GlimpseValueProviderFactory(factories[i]);
            }
        }

        public void ModelBinderProviders()
        {
            var binderProviders = System.Web.Mvc.ModelBinderProviders.BinderProviders;

            for (int i = 0; i < binderProviders.Count; i++)
            {
                binderProviders[i] = new GlimpseBinderProvider(binderProviders[i], Logger);
            }
        }
    }
}