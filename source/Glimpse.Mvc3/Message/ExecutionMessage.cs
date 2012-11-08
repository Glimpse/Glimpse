using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public class ExecutionMessage : MessageBase, IExecutionMessage, ITimerResultMessage
    {
        public ExecutionMessage(FilterCategory? filterCategory, Type filterType, MethodInfo method, TimerResult timerResult, ControllerBase controllerBase)
        {
            // IsChildAction is false if ControllerContext is null
            IsChildAction = controllerBase.ControllerContext != null && controllerBase.ControllerContext.IsChildAction;
            Category = filterCategory;
            ExecutedType = filterType;
            ExecutedMethod = method;
            Duration = timerResult.Duration;
            Offset = timerResult.Offset;
            EventName = Simplify(method.Name);
            EventCategory = filterCategory + "Filter";
        }

        public bool IsChildAction { get; private set; }

        public FilterCategory? Category { get; private set; }

        public Type ExecutedType { get; private set; }

        public MethodInfo ExecutedMethod { get; private set; }

        public string EventName { get; private set; }

        public string EventCategory { get; private set; }
        
        public long Offset { get; private set; }
        
        public TimeSpan Duration { get; private set; }

        private string Simplify(string methodName)
        {
            var nameParts = methodName.Split('.');
            return nameParts[nameParts.Length - 1];
        }
    }
}