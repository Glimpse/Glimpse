using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Core.Tab.Assist;

namespace Glimpse.Core.Tab
{
    /// <summary>
    /// Trace tab
    /// </summary>
    public class Trace : ITab, ITabSetup, IDocumentation, ITabLayout, IKey
    {
        /// <summary>
        /// The tab key that is used during storage.
        /// </summary>
        public const string TabKey = "glimpse_trace";

        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell(0).AsKey().WidthInPixels(100);
                    r.Cell(1);
                    r.Cell(2).WidthInPercent(15).Suffix(" ms").AlignRight().Prefix("T+ ").Class("mono");
                    r.Cell(3).WidthInPercent(15).Suffix(" ms").AlignRight().Class("mono"); 
                }).Build();

        /// <summary>
        /// Gets the name that will show in the tab.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "Trace"; }
        }

        /// <summary>
        /// Gets the documentation URI.
        /// </summary>
        /// <value>The documentation URI.</value>
        public string DocumentationUri
        {
            get { return "http://getglimpse.com/Help/Trace-Tab"; }
        }

        /// <summary>
        /// Gets when the <see cref="ITab.GetData" /> method should run.
        /// </summary>
        /// <value>The execute on.</value>
        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.EndRequest; }
        }

        /// <summary>
        /// Gets the type of the request context that the Tab relies on. If
        /// returns null, the tab can be used in any context.
        /// </summary>
        /// <value>The type of the request context.</value>
        public Type RequestContextType
        {
            get { return null; }
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
        /// Gets the layout which controls the layout.
        /// </summary>
        /// <returns>Object that dictates the layout.</returns>
        public object GetLayout()
        {
            return Layout;
        }

        /// <summary>
        /// Gets the data that should be shown in the UI.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Object that will be shown.</returns>
        public object GetData(ITabContext context)
        {
            var data = context.GetMessages<ITraceMessage>();
            return data;
        }

        /// <summary>
        /// Setups the targeted tab using the specified context.
        /// </summary>
        /// <param name="context">The context which should be used.</param>
        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<ITraceMessage>();
        }
    }
}
