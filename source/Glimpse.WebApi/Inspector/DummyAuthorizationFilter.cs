
using Glimpse.Core.Extensibility;
using System.Web.Http.Controllers;
namespace Glimpse.WebApi.Inspector
{
    internal class DummyAuthorizationFilter : System.Web.Http.Filters.AuthorizationFilterAttribute
    {
        private ILogger _logger;

        public DummyAuthorizationFilter(ILogger logger)
        {
            _logger = logger;
        }
        public override void OnAuthorization(HttpActionContext context)
        {
            _logger.Debug("Dummy Authorization Filter  - OnAuthorization:- Controller:{0} Action:{1} Arguments:{2}",
               context.ControllerContext.ControllerDescriptor.ControllerName,
               context.ActionDescriptor.ActionName,
               context.ActionArguments.Count);
        }
    }
}
