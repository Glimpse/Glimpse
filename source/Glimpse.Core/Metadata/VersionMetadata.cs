using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class VersionMetadata : IMetadata
    {
        public string Key
        {
            get { return "version"; }
        }

        public object GetMetadata(IReadOnlyConfiguration configuration)
        {
            return configuration.Version;
        }
    }
}
