using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.PipelineInspector;
using Glimpse.Core.Extensibility; 

namespace Glimpse.AspNet.Tab
{
    using Glimpse.Core.Plugin.Assist;

    public class Trace : ITab, ITabSetup, IDocumentation, ITabLayout
    {
        public const string TraceMessageStoreKey = "Glimpse.Trace.Messages";
        public const string FirstWatchStoreKey = "Glimpse.Trace.FirstWatch";
        public const string LastWatchStoreKey = "Glimpse.Trace.LastWatch";

        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(100);
                    r.Cell(1);
                    r.Cell(2).WidthInPercent(15).Suffix(" ms");
                    r.Cell(3).WidthInPercent(15).Suffix(" ms");
                }).Build();

        public string Name
        {
            get { return "Trace"; }
        }

        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Plugin/Trace"; }
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public Type RequestContextType
        {
            get { return null; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public object GetData(ITabContext context)
        {
            var data = context.TabStore.Get<IList<TraceModel>>(TraceMessageStoreKey);
            return data;
        }

        public void Setup(ITabSetupContext context)
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<TraceInspector>().Any())
            {
                traceListeners.Add(new TraceInspector(context));
            }
        }
    }
}
