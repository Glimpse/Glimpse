using System.Diagnostics;
using System.Web.Mvc;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Extensions;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Interceptor
{
    internal class InvokeActionResultInterceptor:IInterceptor
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

            using (GlimpseTimer.Start(actionResult.GetType().Name, "MVC"))
            {
                invocation.Proceed();
            }

            watch.Stop();

            calledMetadata.ExecutionTime = watch.Elapsed;
        }
    }
}
