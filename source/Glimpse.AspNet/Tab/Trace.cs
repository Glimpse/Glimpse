using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.PipelineInspector;
using Glimpse.Core.Extensibility; 

namespace Glimpse.AspNet.Tab
{
    public class Trace : ITab, ITabSetup, IDocumentation
    {
        public const string TraceMessageStoreKey = "Glimpse.Trace.Messages";
        public const string FirstWatchStoreKey = "Glimpse.Trace.FirstWatch";
        public const string LastWatchStoreKey = "Glimpse.Trace.LastWatch"; 

        public string Name
        {
            get { return "Trace"; }
        }

        public string DocumentationUri
        {
            // TODO: Update to proper Uri
            get { return "http://localhost/someUrl"; }
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        public Type RequestContextType
        {
            get { return null; }
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
