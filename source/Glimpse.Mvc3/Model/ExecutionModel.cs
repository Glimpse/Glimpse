using System;
using System.Reflection;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.Model
{
    public class ExecutionModel
    {
        public ExecutionModel(IExecutionMessage message) : this(message.IsChildAction, message.Category, message.ExecutedType, message.ExecutedMethod, message.Duration.Milliseconds)
        {
        }

        public ExecutionModel(bool isChildAction, FilterCategory? category, Type executedType, MethodInfo executedMethod, int millisecondsDuration)
        {
            IsChildAction = isChildAction;
            Category = category;
            ExecutedType = executedType;
            ExecutedMethod = executedMethod;
            MillisecondsDuration = millisecondsDuration;
        }

        public int MillisecondsDuration { get; set; }

        public MethodInfo ExecutedMethod { get; set; }

        public Type ExecutedType { get; set; }

        public FilterCategory? Category { get; set; }

        public bool IsChildAction { get; set; }
    }
}