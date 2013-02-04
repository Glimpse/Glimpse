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
    public class TimelineCategoryView : TimelineCategory
    {
        public TimelineCategoryView()
            : base("View", "#10E309", "#0EC41D")
        { 
        }
    }
}
