using System;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents a collection of <see cref="IInspector"/>
    /// </summary>
    public class InspectorsCollection : DiscoverableCollection<IInspector>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InspectorsCollection" /> class
        /// </summary>
        /// <param name="collectionSettings">The collection settings</param>
        /// <param name="logger">The logger</param>
        /// <param name="onChange">Eventhandler to call when the Change event is raised</param>
        public InspectorsCollection(CollectionSettings collectionSettings, ILogger logger, EventHandler onChange = null)
            : base(collectionSettings, logger, onChange)
        {
        }
    }
}