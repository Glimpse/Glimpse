using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Message 
{
    /// <summary>
    /// Used for common events that occur during the event lifecycle.
    /// </summary>
    public class TimelineCategoryRequest : TimelineCategory
    {
        public TimelineCategoryRequest()
            : base("Common", "#AF78DD", "#823BBE")
        { 
        }
    }
}
