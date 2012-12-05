using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message; 

namespace Glimpse.AspNet.Tab
{ 
    public class Timeline : AspNetTab, IDocumentation, ITabSetup, IKey
    {
        private readonly IDictionary<string, TimelineCategoryModel> categories = new Dictionary<string, TimelineCategoryModel>
                           {
                               { "ASP.NET", new TimelineCategoryModel { EventColor = "#AF78DD", EventColorHighlight = "#823BBE" } },
                               { "Controller", new TimelineCategoryModel { EventColor = "#FDBF45", EventColorHighlight = "#DDA431" } },
                               { "Filter", new TimelineCategoryModel { EventColor = "#72A3E4", EventColorHighlight = "#5087CF" } }, 
                               { "View", new TimelineCategoryModel { EventColor = "#10E309", EventColorHighlight = "#0EC41D" } }, 
                           };

        //// .glimpse-panel .glimpse-tl-event-purple { border:1px solid #823BBE; background-color:#AF78DD; } 
        //// .glimpse-panel .glimpse-tl-event-orange { border:1px solid #DDA431; background-color:#FDBF45; } 
        //// .glimpse-panel .glimpse-tl-event-blue { border:1px solid #5087CF; background-color:#72A3E4; } 
        //// .glimpse-panel .glimpse-tl-event-green { border:1px solid #0EC41D; background-color:#10E309; } 
        //// .glimpse-panel .glimpse-tl-event-aqua { border:1px solid #2EBFC7; background-color:#72DEE4; } 
        //// .glimpse-panel .glimpse-tl-event-yellow { border:1px solid #DEE81A; background-color:#F0ED5D; } 
        //// .glimpse-panel .glimpse-tl-event-pink { border:1px solid #DD31DA; background-color:#FD45F7; } 
        //// .glimpse-panel .glimpse-tl-event-red { border:1px solid #DD3131; background-color:#FD4545; }  

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
            get { return "http://getglimpse.com/Help/Plugin/Timeline"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITimelineMessage>();
        }

        public override object GetData(ITabContext context)
        {
            var viewRenderMessages = context.GetMessages<ITimelineMessage>(); 

            var result = new TimelineModel();
            result.Category = categories;

            if (viewRenderMessages != null)
            {
                var maxEndPoint = 0.0;
                var events = new List<TimelineEventModel>();
                foreach (var viewRenderMessage in viewRenderMessages.OrderBy(x => x.Offset))
                {
                    var timelineEvent = new TimelineEventModel();
                    timelineEvent.Title = viewRenderMessage.EventName;
                    timelineEvent.Category = viewRenderMessage.EventCategory;
                    timelineEvent.SubText = viewRenderMessage.EventSubText;
                    timelineEvent.Duration = viewRenderMessage.Duration;
                    timelineEvent.StartPoint = viewRenderMessage.Offset;
                    timelineEvent.StartTime = viewRenderMessage.StartTime;
                    viewRenderMessage.BuildDetails(timelineEvent.Details); 

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
