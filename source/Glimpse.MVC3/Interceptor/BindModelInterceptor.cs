using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Mvc3.Extensions;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Interceptor
{
    internal class BindModelInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            
            var controllerContext = (ControllerContext) invocation.Arguments[0];
            var bindingContext = (ModelBindingContext) invocation.Arguments[1];

            var store = controllerContext.BinderStore();
            var currentProp = store.CurrentProperty;

            if (!currentProp.Name.Equals(bindingContext.ModelName))
            {
                store.MemberOf = "";
                store.CurrentProperty = currentProp = new GlimpseModelBoundProperties { Name = bindingContext.ModelName, Type = bindingContext.ModelType };
            }
            currentProp.ModelBinderType = invocation.TargetType;

            //Trace.Write(string.Format("BINDMODEL ModelName:{0}, ModelType:{1}", bindingContext.ModelName, bindingContext.ModelType), "Selected");
            invocation.Proceed();

            currentProp.RawValue = invocation.ReturnValue;
        }
    }
}
