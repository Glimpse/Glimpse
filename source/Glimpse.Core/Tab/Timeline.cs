using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.Model;

namespace Glimpse.Core.Tab
{
    /// <summary>
    /// Timeline tab
    /// </summary>
    public class Timeline : TabBase, IDocumentation, ITabSetup, IKey
    {
        /// <summary>
        /// Gets the name that will show in the tab.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "Timeline"; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key. Only valid JavaScript identifiers should be used for future compatibility.</value>
        public string Key
        {
            get { return "glimpse_timeline"; }
        }

        /// <summary>
        /// Gets the documentation URI.
        /// </summary>
        /// <value>The documentation URI.</value>
        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Timeline-Tab"; }
        }

        /// <summary>
        /// Setups the targeted tab using the specified context.
        /// </summary>
        /// <param name="context">The context which should be used.</param>
        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITimelineMessage>();
        }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        public override object GetData(ITabContext context)
        {
            var viewRenderMessages = context.GetMessages<ITimelineMessage>(); 

            var result = new TimelineModel();
            result.Category = new Dictionary<string, TimelineCategoryModel>();

            if (viewRenderMessages != null)
            {
                var maxEndPoint = TimeSpan.Zero;
                var events = new List<TimelineEventModel>();
                foreach (var viewRenderMessage in viewRenderMessages.OrderBy(x => x.Offset))
                {
                    if (!result.Category.ContainsKey(viewRenderMessage.EventCategory.Name))
                    {
                        result.Category[viewRenderMessage.EventCategory.Name] = new TimelineCategoryModel { EventColor = viewRenderMessage.EventCategory.Color, EventColorHighlight = viewRenderMessage.EventCategory.ColorHighlight };
                    }

                    var timelineEvent = new TimelineEventModel();
                    timelineEvent.Title = viewRenderMessage.EventName;
                    timelineEvent.Category = viewRenderMessage.EventCategory.Name;
                    timelineEvent.SubText = viewRenderMessage.EventSubText;
                    timelineEvent.Duration = viewRenderMessage.Duration;
                    timelineEvent.StartPoint = viewRenderMessage.Offset;
                    timelineEvent.StartTime = viewRenderMessage.StartTime;
                    //// viewRenderMessage.BuildDetails(timelineEvent.Details); 

                    events.Add(timelineEvent);

                    var endPoint = timelineEvent.EndPoint;
                    if (endPoint > maxEndPoint)
                    {
                        maxEndPoint = endPoint;
                    }
                }

                result.Events = events;
                result.Duration = maxEndPoint;
            }
            
            return result;
        }
    }
}
