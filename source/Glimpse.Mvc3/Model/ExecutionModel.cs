using System;
using System.Reflection;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.Model
{
    public class ExecutionModel
    {
        public ExecutionModel(IActionBaseMessage message)
        {
            IsChildAction = message.IsChildAction;
            ExecutedType = message.ExecutedType;
            ExecutedMethod = message.ExecutedMethod;
            MillisecondsDuration = message.Duration;

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

        public double MillisecondsDuration { get; set; }

        public MethodInfo ExecutedMethod { get; set; }

        public Type ExecutedType { get; set; }

        public FilterCategory? Category { get; set; }

        public FilterBounds? Bounds { get; set; }

        public bool IsChildAction { get; set; }
    }
}