using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Message;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// Options that can used for timeline evetns.
    /// </summary>
    public class Timeline
    {
        private static readonly TimelineCategory request = new TimelineCategoryRequest();
        private static readonly TimelineCategory other = new TimelineCategoryOther();

        /// <summary>
        /// Used for common events that occur during the event lifecycle.
        /// </summary>
        public static TimelineCategory Request
        {
            get { return request; }
        }

        /// <summary>
        /// Used for other events that don't fit in any other given category during the event lifecycle.
        /// </summary>
        public static TimelineCategory Other
        {
            get { return other; }
        }
    }
}
