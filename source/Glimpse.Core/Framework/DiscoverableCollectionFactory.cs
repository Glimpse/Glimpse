using System.Collections.Generic;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public static class DiscoverableCollectionFactory
    {
        public static ICollection<T> Create<T>(DiscoverableCollectionElement config, string globalDiscoveryLocation, ILogger logger)
        {
            var discoverableCollection = new ReflectionDiscoverableCollection<T>(logger);

            discoverableCollection.IgnoredTypes.AddRange(config.IgnoredTypes);

            // Default to config.DiscoveryLocation (collection specific) otherwise overrides 
            // Configuration.DiscoveryLocation (on main <glimpse> node)
            var locationCascade = config.DiscoveryLocation ?? globalDiscoveryLocation;
            if (locationCascade != null)
            {
                discoverableCollection.DiscoveryLocation = locationCascade;
            }

            discoverableCollection.AutoDiscover = config.AutoDiscover;
            if (discoverableCollection.AutoDiscover)
            {
                discoverableCollection.Discover();
            }

            return discoverableCollection;

        }
    }
}