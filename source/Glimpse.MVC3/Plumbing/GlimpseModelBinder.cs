using System.Web.Mvc;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseModelBinder : IModelBinder
    {
        public IModelBinder ModelBinder { get; set; }

        public GlimpseModelBinder(IModelBinder modelBinder)
        {
            ModelBinder = modelBinder;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            var store = controllerContext.BinderStore();
            var currentProp = store.CurrentProperty;

            if (!currentProp.Name.Equals(bindingContext.ModelName))
            {
                store.MemberOf = "";
                store.CurrentProperty = currentProp = new GlimpseModelBoundProperties { Name = bindingContext.ModelName, Type = bindingContext.ModelType };
            }
            currentProp.ModelBinderType = this.GetType().BaseType;

            //Trace.Write(string.Format("BINDMODEL ModelName:{0}, ModelType:{1}", bindingContext.ModelName, bindingContext.ModelType), "Selected");
            var result = ModelBinder.BindModel(controllerContext, bindingContext);

            currentProp.RawValue = result;

            return result;
        }
    }
}
