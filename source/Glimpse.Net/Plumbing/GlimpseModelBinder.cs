using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseModelBinder:IModelBinder
    {
        public IModelBinder ModelBinder { get; set; }

        public GlimpseModelBinder(IModelBinder modelBinder)
        {
            ModelBinder = modelBinder;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var cc = controllerContext;
            var bc = bindingContext;

            Trace.Write("BINDMODEL CALLED ON " + bindingContext.ModelName + bindingContext.FallbackToEmptyPrefix.ToString());

            var result = ModelBinder.BindModel(cc, bc);

            return result;
        }
    }
}
