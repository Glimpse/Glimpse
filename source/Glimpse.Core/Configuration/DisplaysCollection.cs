using System;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class DisplaysCollection : DiscoverableCollection<IDisplay>
    {
        public DisplaysCollection(
            CollectionSettings configuration,
            ILogger logger,
            EventHandler onChange = null)
            : base(configuration, logger, onChange)
        {
        }
    }
}
