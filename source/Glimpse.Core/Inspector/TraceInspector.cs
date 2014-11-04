using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Inspector
{
    /// <summary>
    /// The implementation of <see cref="IInspector"/> for capturing <c>System.Diagnostics.Trace</c> messages.
    /// </summary>
    public class TraceInspector : IInspector
    {
        /// <summary>
        /// Setups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// Executed during initialization of the <see cref="GlimpseRuntime" />
        /// </remarks>
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