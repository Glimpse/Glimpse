using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.Message
{
    public class FilterMessage : ExecutionMessage, IFilterMessage
    {
        public FilterMessage(FilterCategory filterCategory, Type executedType, MethodInfo method, TimerResult timerResult, ControllerBase controllerBase) 
            : base(executedType, method, timerResult)
        {
            IsChildAction = controllerBase.ControllerContext != null && controllerBase.ControllerContext.IsChildAction;
            Category = filterCategory;
            EventCategory = "Filter";
        }

        public FilterCategory Category { get; private set; }

        public Type ResultType { get; set; }

        public override void BuildEvent(ITimelineEvent timelineEvent)
        {
            base.BuildEvent(timelineEvent);

            timelineEvent.Title = string.Format("{0}", Category.ToString());
            timelineEvent.Details.Add("Category", Category.ToString());
            timelineEvent.Details.Add("ResultType", ResultType);
        }
    }
}
