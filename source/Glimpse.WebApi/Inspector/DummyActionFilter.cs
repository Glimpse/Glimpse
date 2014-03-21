using Glimpse.Core.Extensibility;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Glimpse.WebApi.Inspector
{
    internal class DummyActionFilter : ActionFilterAttribute
    {
        private ILogger _logger;

        public DummyActionFilter(ILogger logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(HttpActionContext context)
        {
            _logger.Debug("Dummy Action Filter  - OnActionExecuting:- Controller:{0} Action:{1} Arguments:{2}",
               context.ControllerContext.ControllerDescriptor.ControllerName,
               context.ActionDescriptor.ActionName,
               context.ActionArguments.Count);
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var content = context.Response.Content as ObjectContent;

            _logger.Debug("Dummy Action Filter  - OnActionExecuted:- Controller:{0} Action:{1} Result:{2}",
               context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
               context.ActionContext.ActionDescriptor.ActionName,
               content != null ? content.Value ?? "null" : "null");
        }
    }
}
