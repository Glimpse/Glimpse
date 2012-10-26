using System;
using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    public interface IDiscoverableCollection<T> : ICollection<T>
    {
        bool AutoDiscover { get; }

        string DiscoveryLocation { get; }
        
        void IgnoreType(Type type);
        
        void Discover();
    }
}