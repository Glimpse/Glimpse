using System;
using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    public interface IDiscoverableCollection<T> : ICollection<T>
    {
        bool AutoDiscover { get; }
        void IgnoreType(Type type);
        void Discover();
        string DiscoveryLocation { get; }
    }
}