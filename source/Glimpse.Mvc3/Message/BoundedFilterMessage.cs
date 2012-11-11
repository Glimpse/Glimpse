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
        public BoundedFilterMessage(FilterCategory filterCategory, FilterBounds bounds, Type filterType, MethodInfo method, TimerResult timerResult, ControllerBase controllerBase)
            : base(filterCategory, filterType, method, timerResult, controllerBase)
        {
            Bounds = bounds;
        }

        public FilterBounds Bounds { get; private set; }
    }
}
