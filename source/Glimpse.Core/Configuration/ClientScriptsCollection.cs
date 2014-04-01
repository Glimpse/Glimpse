using System;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class ClientScriptsCollection : DiscoverableCollection<IClientScript>
    {
        public ClientScriptsCollection(
            CollectionSettings configuration,
            ILogger logger,
            EventHandler onChange = null)
            : base(configuration, logger, onChange)
        {
        }
    }
}
