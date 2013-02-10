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
    public class TimelineCategoryFilter : TimelineCategory
    {
        public TimelineCategoryFilter()
            : base("Filter", "#72A3E4", "#5087CF")
        { 
        }
    }
}
