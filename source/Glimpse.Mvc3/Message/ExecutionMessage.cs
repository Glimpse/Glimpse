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
            return GetIsChildAction(controller.ControllerContext);
        }

        protected static bool GetIsChildAction(ControllerContext controllerContext)
        {
            return controllerContext != null && controllerContext.IsChildAction;
        }

        protected static string GetActionName(ActionDescriptor descriptor)
        {
            return descriptor.ActionName;
        }

        protected static string GetActionName(ControllerContext controllerContext)
        {
            return GetActionName(controllerContext.Controller);
        }

        protected static string GetActionName(ControllerBase controller)
        {
            return controller.ValueProvider.GetValue("action").RawValue.ToStringOrDefault();
        }

        protected static string GetControllerName(ActionDescriptor descriptor)
        {
            return descriptor.ControllerDescriptor.ControllerName;
        }

        protected static string GetControllerName(ControllerContext controllerContext)
        {
            return GetControllerName(controllerContext.Controller);
        }

        protected static string GetControllerName(ControllerBase controller)
        {
            return controller.ValueProvider.GetValue("controller").RawValue.ToStringOrDefault();
        }

        protected static Type GetExecutedType(ActionDescriptor descriptor)
        {
            return descriptor.ControllerDescriptor.ControllerType;
        }
    }
}