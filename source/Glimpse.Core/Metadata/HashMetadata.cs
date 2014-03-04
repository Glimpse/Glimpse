using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public object GetMetadata(IReadonlyConfiguration configuration)
        {
            return configuration.Hash;
        }
    }
}
