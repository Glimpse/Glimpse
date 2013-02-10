using System;
using System.Reflection;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.Model
{
    public class ExecutionModel
    {
        public ExecutionModel(IExecutionMessage message)
        {
            IsChildAction = message.IsChildAction;
            ExecutedType = message.ExecutedType;
            ExecutedMethod = message.ExecutedMethod;
            Duration = message.Duration;
            ActionName = message.ActionName;
            ControllerName = message.ControllerName;

            var filter = message as IFilterMessage;
            if (filter != null)
            {
                Category = filter.Category;
            }

            var bounds = message as IBoundedFilterMessage;
            if (bounds != null)
            {
                Bounds = bounds.Bounds;
            }
        }

        public TimeSpan Duration { get; set; }

        public MethodInfo ExecutedMethod { get; set; }

        public Type ExecutedType { get; set; }

        public FilterCategory? Category { get; set; }

        public FilterBounds? Bounds { get; set; }

        public bool IsChildAction { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }
}