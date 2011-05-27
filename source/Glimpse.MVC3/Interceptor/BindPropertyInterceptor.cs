using System.ComponentModel;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Mvc3.Extensions;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Interceptor
{
    internal class BindPropertyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
            var controllerContext = (ControllerContext) invocation.Arguments[0];
            var propertyDescriptor = (PropertyDescriptor) invocation.Arguments[2];

            var store = controllerContext.BinderStore();
            store.CurrentProperty = new GlimpseModelBoundProperties { Name = propertyDescriptor.Name, Type = propertyDescriptor.PropertyType, ModelBinderType = invocation.TargetType};

            invocation.Proceed();
        }
    }
}
