using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Trace : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public const string TraceMessageStoreKey = "Glimpse.Trace.Messages";
        public const string FirstWatchStoreKey = "Glimpse.Trace.FirstWatch";
        public const string LastWatchStoreKey = "Glimpse.Trace.LastWatch";

        public string Name
        {
            get { return "Trace"; }
        }

        public object GetData(HttpContextBase context)
        {
            var messages = context.Items[TraceMessageStoreKey] as IList<IList<string>>;
            if (messages == null) return null;

            return messages;
        }

        public void SetupInit()
        {
            var traceListeners = System.Diagnostics.Trace.Listeners;
            if (!traceListeners.OfType<GlimpseTraceListener>().Any())
                traceListeners.Add(new GlimpseTraceListener()); //Add trace listener if it isn't already configured
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Trace"; }
        }
    }
}