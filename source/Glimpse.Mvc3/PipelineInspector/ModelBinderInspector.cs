using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using ValueProviderFactory = Glimpse.Mvc.AlternateImplementation.ValueProviderFactory;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ModelBinderInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            SetupModelBinderProviders(context);
            SetupValueProviderFactories(context);
            SetupModelBinders(context);
        }

        private void SetupModelBinderProviders(IPipelineInspectorContext context)
        {
#if !MVC2
            var binderProviders = ModelBinderProviders.BinderProviders;
            if (binderProviders.Count == 0)
            {
                return;
            }

            var alternateModelBinderProvider = new ModelBinderProvider(context.ProxyFactory);

            for (int i = 0; i < binderProviders.Count; i++)
            {
                var originalBinder = binderProviders[i];
                IModelBinderProvider newProvider;

                if (alternateModelBinderProvider.TryCreate(originalBinder, out newProvider))
                {
                    context.Logger.Info(Resources.ModelBinderInspectorSetupReplacedModelBinderProvider, originalBinder.GetType());
                    binderProviders[i] = newProvider;
                }
            }
#endif
        }

        private void SetupValueProviderFactories(IPipelineInspectorContext context)
        {
            var factories = ValueProviderFactories.Factories;
            if (factories.Count == 0)
            {
                return;
            }

            var alternateValueProviderFactory = new ValueProviderFactory(context.ProxyFactory);

            for (int i = 0; i < factories.Count; i++)
            {
                var originalFactory = factories[i];
                System.Web.Mvc.ValueProviderFactory newFactory;

                if (alternateValueProviderFactory.TryCreate(originalFactory, out newFactory))
                {
                    context.Logger.Info(Resources.ModelBinderInspectorSetupReplacedValueProviderFactory, originalFactory.GetType());
                    factories[i] = newFactory;
                }
            }
        }

        private void SetupModelBinders(IPipelineInspectorContext context)
        {
            var alternateModelBinder = new ModelBinder(context.ProxyFactory);
            var binders = ModelBinders.Binders;
            var keys = binders.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                var type = keys[i];
                var originalBinder = binders[type] as DefaultModelBinder; // TODO: Handle IModelBinders as well
                DefaultModelBinder newBinder;

                if (originalBinder != null && alternateModelBinder.TryCreate(originalBinder, out newBinder))
                {
                    context.Logger.Info(Resources.ModelBinderInspectorSetupReplacedModelBinder, originalBinder.GetType());
                    binders[type] = newBinder;
                }
            }

            // handle default binder
            var originalDefaultBinder = ModelBinders.Binders.DefaultBinder as DefaultModelBinder;
            DefaultModelBinder newDefaultBinder;
            if (originalDefaultBinder != null && alternateModelBinder.TryCreate(originalDefaultBinder, out newDefaultBinder))
            {
                context.Logger.Info(Resources.ModelBinderInspectorSetupReplacedModelBinder, originalDefaultBinder.GetType());
                ModelBinders.Binders.DefaultBinder = newDefaultBinder;
            }
        }
    }
}