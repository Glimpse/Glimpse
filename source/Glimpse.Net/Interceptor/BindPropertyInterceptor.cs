using System.ComponentModel;
using System.Web.Mvc;
using Castle.Core.Interceptor;
using Glimpse.Net.Extensions;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Interceptor
{
    internal class BindPropertyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
            var controllerContext = (ControllerContext) invocation.Arguments[0];
            var propertyDescriptor = (PropertyDescriptor) invocation.Arguments[2];

            var store = controllerContext.BinderStore();
            store.CurrentProperty = new GlimpseModelBoundProperties { Name = propertyDescriptor.Name, Type = propertyDescriptor.PropertyType };

            invocation.Proceed();
        }
    }
}
