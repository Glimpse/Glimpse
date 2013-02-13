using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.PipelineInspector
{
    public class TraceInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<TraceListener>().Any())
            {
                traceListeners.Add(new TraceListener(context.MessageBroker, context.TimerStrategy));
            }
        }
    }
}