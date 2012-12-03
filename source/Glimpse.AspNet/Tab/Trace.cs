using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.PipelineInspector;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.Tab
{
    public class Trace : ITab, ITabSetup, IDocumentation, ITabLayout, IKey
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

        public string Key
        {
            get { return "glimpse_trace"; }
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
            // TODO: This seems like it would fit better in an IPipeline inspector. No?
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<TraceInspector>().Any())
            {
                traceListeners.Add(new TraceInspector(context));
            }
        }
    }
}
