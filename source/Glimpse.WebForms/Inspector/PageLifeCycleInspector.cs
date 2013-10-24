using System;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.WebForms.Message;

namespace Glimpse.WebForms.Inspector
{
    public class PageLifeCycleInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            MessageBroker = context.MessageBroker;
            context.MessageBroker.Subscribe<ITraceMessage>(ProcessMessage);
        }

        private IMessageBroker MessageBroker { get; set; }

        private void ProcessMessage(ITraceMessage message)
        {
            if (message.Category == "ms")
            {
                if (message.Message.Contains("Begin"))
                {
                    HttpContext.Current.Items["_GlimpseWebFormPageLifeCycleEvent"] = message;
                    HttpContext.Current.Items["_GlimpseWebFormPageLifeCycleEventDateTime"] = DateTime.Now; 
                }
                else if (message.Message.Contains("End") && HttpContext.Current.Items.Contains("_GlimpseWebFormPageLifeCycleEvent"))
                {
                    var beginMessage = (ITraceMessage)HttpContext.Current.Items["_GlimpseWebFormPageLifeCycleEvent"];

                    GenerateMessage(beginMessage, message, (DateTime)HttpContext.Current.Items["_GlimpseWebFormPageLifeCycleEventDateTime"]);

                    HttpContext.Current.Items.Remove("_GlimpseWebFormPageLifeCycleEvent");
                }
            }
        }

        private void GenerateMessage(ITraceMessage beginMessage, ITraceMessage endMessage, DateTime startTime)
        {
            var item = new PageLifeCycleMessage();
            item.EventName = ProcessEventName(beginMessage.Message);
            item.StartTime = startTime;
            item.Offset = beginMessage.FromFirst;
            item.FromLast = beginMessage.FromLast;
            item.Duration = endMessage.FromFirst - beginMessage.FromFirst;
            item.EventCategory = WebFormsTimelineCategory.WebForms;

            MessageBroker.Publish(item);
        }

        private string ProcessEventName(string title)
        {
            var lastIndex = title.LastIndexOf(' ');
            return title.Substring(lastIndex, title.Length - lastIndex);
        }
    }
}
