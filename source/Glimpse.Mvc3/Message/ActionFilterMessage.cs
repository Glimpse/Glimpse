using System;
using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.Message
{
    public class ActionFilterMessage : FilterMessage, IActionBasedMessage
    {
        public ActionFilterMessage(TimerResult timerResult, string controllerName, string actionName, FilterCategory filterCategory, Type resultType, bool isChildAction, Type executedType, MethodInfo method, string eventName = null, string eventCategory = null)
            : base(timerResult, filterCategory, resultType, isChildAction, executedType, method, eventName, eventCategory)
        {
            ControllerName = controllerName;
            ActionName = actionName; 

            if (string.IsNullOrEmpty(eventName))
            {
                EventName = string.Format("{0} - {1}:{2}", Category.ToString(), ControllerName, ActionName);
            }
        }

        public string ControllerName { get; protected set; }

        public string ActionName { get; protected set; }
    }
}