using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Mvc.Message
{
    public class FilterMessage : ActionBaseMessage, IFilterMessage, IExecutionMessage
    {
        public FilterMessage(TimerResult timerResult, FilterCategory filterCategory, Type resultType, bool isChildAction, Type executedType, MethodInfo method, string eventName = null, Core.Message.TimelineCategory eventCategory = null)
            : base(timerResult, isChildAction, executedType, method, eventName, eventCategory) 
        {
            Category = filterCategory;
            ResultType = resultType;

            if (string.IsNullOrEmpty(eventName))
            {
                EventName = string.Format("{0}", Category.ToString());
            }

            if (eventCategory == null)
            {
                EventCategory = Timeline.Filter;
            }  
        }

        public FilterCategory Category { get; protected set; }

        public Type ResultType { get; protected set; }

        public override void BuildDetails(IDictionary<string, object> details)
        {
            base.BuildDetails(details);

            details.Add("ResultType", ResultType);
        }

        protected static Type GetResultType(object result)
        {
            return result.GetTypeOrNull();
        } 
    }
}
