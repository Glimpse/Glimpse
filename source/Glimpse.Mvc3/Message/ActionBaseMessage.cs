using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public class ActionBaseMessage : TimelineMessage, IActionBaseMessage
    {
        public ActionBaseMessage(TimerResult timerResult, bool isChildAction, Type executedType, MethodInfo method, string eventName = null, string eventCategory = null)
            : base(timerResult, executedType, method, eventName, eventCategory)
        {
            IsChildAction = isChildAction;
        }

        public bool IsChildAction { get; protected set; }

        public override void BuildDetails(IDictionary<string, object> details)
        {
            base.BuildDetails(details);

            details.Add("IsChildAction", IsChildAction);
            if (ExecutedMethod != null)
            {
                details.Add("ExecutionMethod", ExecutedMethod.Name);
            }

            if (ExecutedType != null)
            {
                details.Add("ExecutedType", ExecutedType);
            }
        }

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
            return GetValueProviderEntry(controller, "action");
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
            return GetValueProviderEntry(controller, "controller");
        }

        protected static Type GetExecutedType(ActionDescriptor descriptor)
        {
            return descriptor.ControllerDescriptor.ControllerType;
        }

        protected static string GetValueProviderEntry(ControllerBase controller, string key)
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