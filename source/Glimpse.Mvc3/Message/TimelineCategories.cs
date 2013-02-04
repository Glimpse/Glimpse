using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    /// <summary>
    /// Options that can used for timeline evetns.
    /// </summary>
    public class Timeline : Core.Message.Timeline
    {
        private static readonly TimelineCategory controller = new TimelineCategoryController();
        private static readonly TimelineCategory filter = new TimelineCategoryFilter();
        private static readonly TimelineCategory view = new TimelineCategoryView();

        /// <summary>
        /// Used for common events that occur during the event lifecycle.
        /// </summary>
        public static TimelineCategory Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Used for common events that occur during the event lifecycle.
        /// </summary>
        public static TimelineCategory Filter
        {
            get { return filter; }
        }

        /// <summary>
        /// Used for common events that occur during the event lifecycle.
        /// </summary>
        public static TimelineCategory View
        {
            get { return view; }
        }
    }
}
