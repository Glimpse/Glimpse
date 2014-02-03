#if NET45Plus
using Glimpse.Core.Extensibility;
using Glimpse.WebForms.AlternateType;
using System.Web.ModelBinding;

namespace Glimpse.WebForms.Inspector
{
    public class ModelBinderInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var originalDefaultBinder = ModelBinders.Binders.DefaultBinder;
            IModelBinder newDefaultBinder;
            var alternateModelBinder = new ModelBinder(context.ProxyFactory);
            if (originalDefaultBinder != null && alternateModelBinder.TryCreate(originalDefaultBinder, out newDefaultBinder))
            {
                context.Logger.Info(Resources.ModelBinderInspectorSetupReplacedModelBinder, originalDefaultBinder.GetType());
                ModelBinders.Binders.DefaultBinder = newDefaultBinder;
            }
        }
    }
}
#endif