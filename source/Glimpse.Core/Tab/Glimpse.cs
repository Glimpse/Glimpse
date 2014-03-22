using System.Collections.Specialized;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Tab
{
    /// <summary>
    /// Glimpse tab
    /// </summary>
    public class Glimpse : TabBase, ITabSetup
    {
        /// <summary>
        /// The tab key that is used during storage.
        /// </summary>
        public const string TabKey = "glimpse_glimpse";

        /// <summary>
        /// Gets the name that will show in the tab.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "Glimpse"; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key. Only valid JavaScript identifiers should be used for future compatibility.</value>
        public string Key
        {
            get { return TabKey; }
        }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        public override object GetData(ITabContext context)
        {
            lock (activeGlimpseRequestContextEvents)
            {
                var totalNumberOfActiveGlimpseRequestContexts = activeGlimpseRequestContextEvents.Count;
                var oldestActiveGlimpseRequestContext = (ActiveGlimpseRequestContextEventArgs)(totalNumberOfActiveGlimpseRequestContexts != 0 ? activeGlimpseRequestContextEvents[0] : null);
                var newestActiveGlimpseRequestContext = (ActiveGlimpseRequestContextEventArgs)(totalNumberOfActiveGlimpseRequestContexts != 0 ? activeGlimpseRequestContextEvents[totalNumberOfActiveGlimpseRequestContexts - 1] : null);

                return new
                {
                    TotalNumberOfActiveGlimpseRequestContexts = totalNumberOfActiveGlimpseRequestContexts,
                    OldestActiveGlimpseRequestContext = oldestActiveGlimpseRequestContext == null ? null : new
                    {
                        RequestId = oldestActiveGlimpseRequestContext.GlimpseRequestId,
                        AddedOn = oldestActiveGlimpseRequestContext.RaisedOn
                    },
                    NewestActiveGlimpseRequestContext = newestActiveGlimpseRequestContext == null ? null : new
                    {
                        RequestId = newestActiveGlimpseRequestContext.GlimpseRequestId,
                        AddedOn = newestActiveGlimpseRequestContext.RaisedOn
                    },
                };
            }
        }

        private static readonly OrderedDictionary activeGlimpseRequestContextEvents = new OrderedDictionary();

        /// <summary>
        /// Setups the targeted tab using the specified context.
        /// </summary>
        /// <param name="context">The context which should be used.</param>
        public void Setup(ITabSetupContext context)
        {
            ActiveGlimpseRequestContexts.RequestContextAdded +=
                (sender, eventArgs) =>
                {
                    lock (activeGlimpseRequestContextEvents)
                    {
                        activeGlimpseRequestContextEvents.Add(eventArgs.GlimpseRequestId, eventArgs);
                    }
                };

            ActiveGlimpseRequestContexts.RequestContextRemoved +=
                (sender, eventArgs) =>
                {
                    lock (activeGlimpseRequestContextEvents)
                    {
                        activeGlimpseRequestContextEvents.Remove(eventArgs.GlimpseRequestId);
                    }
                };
        }
    }
}
