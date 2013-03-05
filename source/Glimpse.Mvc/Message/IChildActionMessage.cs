using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public interface IChildActionMessage : IActionMessage
    {
        bool IsChildAction { get; set; }
    }

    public static class ChildActionMessageExtension
    {
        public static T AsChildActionMessage<T>(this T message, ControllerContext controllerContext)
            where T : IChildActionMessage
        {
            message.IsChildAction = controllerContext != null && controllerContext.IsChildAction;

            return message;
        }

        public static T AsChildActionMessage<T>(this T message, ControllerBase controller)
            where T : IChildActionMessage
        {
            message.AsChildActionMessage(controller.ControllerContext);

            return message;
        } 
    }
}
