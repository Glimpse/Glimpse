using System;
using System.Collections.Generic; 
using System.Linq;
using System.Reflection;  
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.Tab.Assist;
using Glimpse.WebForms.Model;

namespace Glimpse.WebForms.Tab
{
    public class Execution : AspNetTab, ITabSetup, ITabLayout, IKey
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell("{{ordinal}}").AsKey().WidthInPixels(60).WithTitle("Ordinal"); 
                    r.Cell("{{event}}").AsKey().WithTitle("Event"); 
                    r.Cell("{{duration}}").WidthInPercent(15).Suffix(" ms").AlignRight().Class("mono").WithTitle("Duration");
                    r.Cell("{{fromFirst}}").WidthInPercent(15).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono").WithTitle("From Request Start");
                    r.Cell("{{fromLast}}").WidthInPercent(15).Suffix(" ms").AlignRight().Class("mono").WithTitle("From Last");
                }).Build();
        
        public override string Name
        {
            get { return "Execution"; }
        }

        public string Key
        {
            get { return "glimpse_webforms_execution"; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public void Setup(ITabSetupContext context)
        { 
            //Make sure the traces are dumpted out to the normal trace stream
            var writeToDiagnosticsTraceField = typeof(System.Web.TraceContext).GetField("_writeToDiagnosticsTrace", BindingFlags.Static | BindingFlags.NonPublic);
            writeToDiagnosticsTraceField.SetValue(null, true);

            context.PersistMessages<ITraceMessage>();
        }

        public override object GetData(ITabContext context)
        {
            var data = ProcessData(context.GetMessages<ITraceMessage>(), context.Logger);
            return data;
        }

        private object ProcessData(IEnumerable<ITraceMessage> traceMessages, ILogger logger)
        {
            var enumerable = traceMessages.ToList();

            logger.Debug("Start processing trace messages for WebFormsExecution - Count {0}", enumerable.Count);

            var webFormsMessages = enumerable.Where(x => x.Category == "ms").ToList(); 
            if (webFormsMessages.Count > 0)
            {
                logger.Debug("Found {0} specific messages", webFormsMessages.Count);

                var result = new List<ExecutionItemModel>();
                var x = 0;
                for (var i = 0; i < webFormsMessages.Count; i += 2)
                {
                    var message1 = webFormsMessages[i];
                    var message2 = webFormsMessages[i + 1];
                     
                    var item = new ExecutionItemModel();
                    item.Ordinal = x++;
                    item.Event = ProcessEventName(message1.Message);
                    item.FromFirst = message1.FromFirst;
                    item.FromLast = message1.FromLast;
                    item.Duration = message2.FromFirst - message1.FromFirst;

                    result.Add(item);
                }

                logger.Debug("Matched {0} events", result.Count);

                return result;
            }

            logger.Debug("Finish processing trace messages for WebFormsExecution");
             
            return "Error in processing messages";
        }

        private string ProcessEventName(string title)
        {
            var lastIndex = title.LastIndexOf(' ');
            return title.Substring(lastIndex, title.Length - lastIndex);
        }

    }
}
