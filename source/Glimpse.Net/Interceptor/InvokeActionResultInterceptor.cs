using System.Diagnostics;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Net.Extensions;
using Glimpse.Net.Plumbing;

namespace Glimpse.Net.Interceptor
{
    class InvokeActionResultInterceptor:IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
            var controllerContext = (ControllerContext) invocation.Arguments[0];
            var actionResult = (ActionResult)invocation.Arguments[1];

            var allFilters = controllerContext.FiltersStore();
            var calledFilters = controllerContext.CallStore();

            var action = GlimpseFilterCallMetadata.ActionResult(actionResult, controllerContext.IsChildAction);
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
