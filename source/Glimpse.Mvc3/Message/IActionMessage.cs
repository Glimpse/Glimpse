using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IActionMessage : IMessage
    {
        string ControllerName { get; set; }

        string ActionName { get; set; }
    }

    public static class ActionMessageExtension
    {
        public static T AsActionMessage<T>(this T message, ActionDescriptor descriptor)
            where T : IActionMessage
        {
            message.ControllerName = descriptor.ControllerDescriptor.ControllerName;
            message.ActionName = descriptor.ActionName;

            return message;
        }

        public static T AsActionMessage<T>(this T message, ControllerContext controllerContext)
            where T : IActionMessage
        {
            message.AsActionMessage(controllerContext.Controller);

            return message;
        }

        public static T AsActionMessage<T>(this T message, ControllerBase controller)
            where T : IActionMessage
        {
            message.ControllerName = GetValueProviderEntry(controller, "controller");
            message.ActionName = GetValueProviderEntry(controller, "action"); 

            return message;
        }

        private static string GetValueProviderEntry(ControllerBase controller, string key)
        {
            var result = string.Empty;
            if (controller != null && controller.ValueProvider != null)
            {
                var resultObject = controller.ValueProvider.GetValue(key);
                if (resultObject != null)
                {
                    result = resultObject.RawValue.ToStringOrDefault();
                }
            }

            return result;
        }
    }
}
