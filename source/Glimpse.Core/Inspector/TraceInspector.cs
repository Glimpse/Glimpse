using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Inspector
{
    public class TraceInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<TraceListener>().Any())
            {
                traceListeners.Add(new TraceListener(context.MessageBroker, context.TimerStrategy));
            }
        }
    }
}