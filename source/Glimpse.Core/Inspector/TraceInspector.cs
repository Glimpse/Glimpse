using System.Linq;
using Glimpse.Core.Extensibility;

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
        /// Executed during the <see cref="Glimpse.Core.Framework.IGlimpseRuntime.Initialize" /> phase of
        /// system startup. Specifically, with the ASP.NET provider, this is wired to/implemented by the
        /// <c>System.Web.IHttpModule.Init</c> method.
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