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
        public FilterMessage(FilterCategory filterCategory, Type filterType, MethodInfo method, TimerResult timerResult, ControllerBase controllerBase) 
            : base(filterType, method, timerResult, controllerBase)
        {
            Category = filterCategory; 
        }

        public FilterCategory Category { get; private set; }

        public Type ResultType { get; set; }
    }
}
