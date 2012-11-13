using System;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.Message
{
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