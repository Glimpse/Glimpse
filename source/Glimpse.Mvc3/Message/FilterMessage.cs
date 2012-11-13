using System;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Mvc.Message
{
    public class FilterMessage : ExecutionMessage, IFilterMessage
    {
        public FilterMessage(TimerResult timerResult, FilterCategory filterCategory, Type resultType, bool isChildAction, Type executedType, MethodInfo method, string eventName = null, string eventCategory = null)
            : base(timerResult, isChildAction, executedType, method, eventName, eventCategory) 
        {
            Category = filterCategory;
            ResultType = resultType;

            if (string.IsNullOrEmpty(eventName))
            {
                EventName = string.Format("{0}", Category.ToString());
            }

            if (string.IsNullOrEmpty(eventCategory))
            {
                EventCategory = "Filter";
            }  
        }

        public FilterCategory Category { get; protected set; }

        public Type ResultType { get; protected set; }

        public override void BuildEvent(ITimelineEvent timelineEvent)
        {
            base.BuildEvent(timelineEvent);

            timelineEvent.Details.Add("Category", Category.ToString());
            timelineEvent.Details.Add("ResultType", ResultType);
        }

        protected static Type GetResultType(object result)
        {
            return result.GetTypeOrNull();
        } 
    }
}
