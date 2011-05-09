using System.Diagnostics;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Net.Extensions;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Interceptor
{
    internal class InvokeActionMethodInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)

            var controllerContext = (ControllerContext)invocation.Arguments[0];
            var actionDescriptor = (ActionDescriptor)invocation.Arguments[1];

            var allFilters = controllerContext.FiltersStore();
            var calledFilters = controllerContext.CallStore();

            var action = GlimpseFilterCallMetadata.ControllerAction(actionDescriptor, controllerContext.IsChildAction);
            allFilters.Add(action);

            var calledMetadata = new GlimpseFilterCalledMetadata { Guid = action.Guid};
            calledFilters.Add(calledMetadata);

            var watch = new Stopwatch();
            watch.Start();

            invocation.Proceed();

            watch.Stop();

            calledMetadata.ExecutionTime = watch.Elapsed;
        }
    }
}
