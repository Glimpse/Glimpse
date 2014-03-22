using System;
using System.Collections.Generic; 
using System.Linq;
using System.Reflection;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.Tab.Assist;
using Glimpse.WebForms.Inspector;
using Glimpse.WebForms.Model;

namespace Glimpse.WebForms.Tab
{
    public class PageLifeCycle : AspNetTab, ITabSetup, ITabLayout, IKey
    {
        private static readonly FieldInfo writeToDiagnosticsTraceField = typeof(System.Web.TraceContext).GetField("_writeToDiagnosticsTrace", BindingFlags.Static | BindingFlags.NonPublic);

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
            get { return "Page Life Cycle"; }
        }

        public string Key
        {
            get { return "glimpse_webforms_execution"; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginSessionAccess | RuntimeEvent.EndRequest; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<PageLifeCycleMessage>();
        }

        public override object GetData(ITabContext context)
        {
            var hasRun = context.TabStore.Get("hasRun");
            if (hasRun == null)
            { 
                context.TabStore.Set("hasRun", "true");
                 
                // Make sure the traces are dumpted out to the normal trace stream  
                writeToDiagnosticsTraceField.SetValue(null, true);    

                return null;
            }

            var data = ProcessData(context.GetMessages<PageLifeCycleMessage>());
            return data;
        }

        private object ProcessData(IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
        {
            var x = 0;
            var result = new List<ExecutionItemModel>();
            foreach (var pageLifeCycleMessage in pageLifeCycleMessages)
            {
                var item = new ExecutionItemModel();
                item.Ordinal = x++;
                item.Event = pageLifeCycleMessage.EventName;
                item.FromFirst = pageLifeCycleMessage.Offset;
                item.FromLast = pageLifeCycleMessage.FromLast;
                item.Duration = pageLifeCycleMessage.Duration;

                result.Add(item);
            }

            return result;
        }
    }
}
