using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Message 
{
    /// <summary>
    /// Used for other events that don't fit in any other given category during the event lifecycle.
    /// </summary>
    public class TimelineCategoryOther : TimelineCategory
    {
        public TimelineCategoryOther()
            : base("Other", "#EEEEEE", "#CCCCCC")
        { 
        }
    }
}
