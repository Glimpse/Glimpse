using System;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents a collection of <see cref="ITab"/>
    /// </summary>
    public class TabsCollection : DiscoverableCollection<ITab>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabsCollection" /> class
        /// </summary>
        /// <param name="collectionSettings">The collection settings</param>
        /// <param name="logger">The logger</param>
        /// <param name="onChange">Eventhandler to call when the Change event is raised</param>
        public TabsCollection(CollectionSettings collectionSettings, ILogger logger, EventHandler onChange = null)
            : base(collectionSettings, logger, onChange)
        {
        }
    }
}