using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.Model;

namespace Glimpse.Core.Tab
{
    public class Timeline : TabBase, IDocumentation, ITabSetup, IKey
    {  
        public override string Name
        {
            get { return "Timeline"; }
        }

        public string Key
        {
            get { return "glimpse_timeline"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Timeline-Tab"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITimelineMessage>();
        }

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
