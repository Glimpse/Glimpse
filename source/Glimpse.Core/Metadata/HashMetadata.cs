using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class HashMetadata : IMetadata
    {
        public string Key
        {
            get { return "hash"; }
        }

        public object GetMetadata(IReadOnlyConfiguration configuration)
        {
            return configuration.Hash;
        }
    }
}
