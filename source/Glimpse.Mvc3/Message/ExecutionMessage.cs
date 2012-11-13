using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public class ExecutionMessage : TimelineMessage, IExecutionMessage
    {
        public ExecutionMessage(TimerResult timerResult, bool isChildAction, Type executedType, MethodInfo method, string eventName = null, string eventCategory = null)
            : base(timerResult, eventName, eventCategory) 
        {
            IsChildAction = isChildAction;
            ExecutedType = executedType;
            ExecutedMethod = method; 
        }

        public MethodInfo ExecutedMethod { get; protected set; } 

        public Type ExecutedType { get; protected set; }

        public bool IsChildAction { get; protected set; } 

        protected static bool GetIsChildAction(ControllerBase controller)
        {
            return controller.ControllerContext != null && controller.ControllerContext.IsChildAction;
        }

        protected static string GetActionName(ActionDescriptor descriptor)
        {
            return descriptor.ActionName;
        }

        protected static string GetActionName(ControllerBase controller)
        {
            return controller.ValueProvider.GetValue("action").RawValue.ToStringOrDefault();
        }

        protected static string GetControllerName(ActionDescriptor descriptor)
        {
            return descriptor.ControllerDescriptor.ControllerName;
        }

        protected static string GetControllerName(ControllerBase controller)
        {
            return controller.ValueProvider.GetValue("controller").RawValue.ToStringOrDefault(); ;
        }
    }

    public class ActionMessage : ExecutionMessage, IActionBasedMessage
    {
        public ActionMessage(TimerResult timerResult, string controllerName, string actionName, bool isChildAction, Type executedType, MethodInfo method, string eventName = null, string eventCategory = null)
            : base(timerResult, isChildAction, executedType, method, eventName, eventCategory)
        {
            ControllerName = controllerName;
            ActionName = actionName; 

            if (string.IsNullOrEmpty(eventName))
            {
                EventName = string.Format(" {0}:{1}", ControllerName, ActionName);
            }
        }

        public string ControllerName { get; protected set; }

        public string ActionName { get; protected set; }
    }
}