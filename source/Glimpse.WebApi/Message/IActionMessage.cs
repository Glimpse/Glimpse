using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.WebApi.Message
{
    public interface IActionMessage : IMessage
    {
        string ControllerName { get; set; }

        string ActionName { get; set; }
    }

    public static class ActionMessageExtension
    {
        public static T AsActionMessage<T>(this T message, HttpActionDescriptor descriptor)
            where T : IActionMessage
        {
            message.ControllerName = descriptor.ControllerDescriptor.ControllerName;
            message.ActionName = descriptor.ActionName;

            return message;
        }

        public static T AsActionMessage<T>(this T message, HttpControllerContext controllerContext)
            where T : IActionMessage
        {
            var controller = controllerContext.Controller as ApiController;
            if (controller != null)
            {
                message.AsActionMessage(controller);
            }

            return message;
        }

        public static T AsActionMessage<T>(this T message, ApiController controller)
            where T : IActionMessage
        {
            message.ControllerName = controller.ControllerContext.ControllerDescriptor.ControllerName;
            message.ActionName = controller.ActionContext.ActionDescriptor.ActionName;

            return message;
        }

        public static T AsActionMessage<T>(this T message, string controllerName, string actionName)
            where T : IActionMessage
        {
            message.ControllerName = controllerName;
            message.ActionName = actionName;

            return message;
        }
    }
}
