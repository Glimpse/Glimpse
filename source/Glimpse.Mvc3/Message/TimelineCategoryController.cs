using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message 
{
    /// <summary>
    /// Used for common events that occur during the event lifecycle.
    /// </summary>
    public class TimelineCategoryController : TimelineCategory
    {
        public TimelineCategoryController()
            : base("Controller", "#FDBF45", "#DDA431")
        { 
        }
    }
}
