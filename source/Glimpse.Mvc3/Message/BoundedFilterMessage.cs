using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.Message
{
    public class BoundedFilterMessage : FilterMessage, IBoundedFilterMessage
    {
        public BoundedFilterMessage(FilterCategory filterCategory, FilterBounds bounds, Type executedType, MethodInfo method, TimerResult timerResult, ControllerBase controllerBase)
            : base(filterCategory, executedType, method, timerResult, controllerBase)
        {
            Bounds = bounds;
        }

        public FilterBounds Bounds { get; private set; }

        public string ControllerName { get; protected set; }

        public string ActionName { get; protected set; }

        public override void BuildEvent(ITimelineEvent timelineEvent)
        {
            base.BuildEvent(timelineEvent);

            timelineEvent.Title = string.Format("{0}:{1} - {2}:{3}", Category.ToString(), Bounds.ToString(), ControllerName, ActionName); 
            timelineEvent.Details.Add("Bounds", Bounds.ToString());
        }
    }
}
