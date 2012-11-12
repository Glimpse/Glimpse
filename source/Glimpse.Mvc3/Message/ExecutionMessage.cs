using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    public class ExecutionMessage : TimerResultMessage, IExecutionMessage
    {
        public ExecutionMessage(Type executedType, MethodInfo method, TimerResult timerResult)
            : base(timerResult)
        {
            ExecutedType = executedType;
            ExecutedMethod = method;
            EventName = Simplify(method.Name);
            EventCategory = "Controller";
        }

        public bool IsChildAction { get; protected set; }

        public Type ExecutedType { get; private set; }

        public MethodInfo ExecutedMethod { get; private set; } 

        private string Simplify(string methodName)
        {
            var nameParts = methodName.Split('.');
            return nameParts[nameParts.Length - 1];
        }
    }
}