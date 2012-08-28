using System;
using System.Web.Mvc;

namespace Glimpse.Mvc3.AlternateImplementation
{
    public class ActionInvoker
    {
        public class InvokeActionMethod
        {
            public class Message
            {
                public Message(AsyncActionInvoker.Arguments arguments, ActionResult returnValue)
                {
                    var controllerDescriptor = arguments.ActionDescriptor.ControllerDescriptor;

                    ControllerName = controllerDescriptor.ControllerName;
                    ControllerType = controllerDescriptor.ControllerType;
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    ActionName = arguments.ActionDescriptor.ActionName;
                    ActionResultType = returnValue.GetType();
                }

                public string ControllerName { get; set; }
                public Type ControllerType { get; set; }
                public bool IsChildAction { get; set; }
                public string ActionName { get; set; }
                public Type ActionResultType { get; set; }
            }
        }
    }
}