using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public object GetMetadata(IReadonlyConfiguration configuration)
        {
            return GlimpseRuntime.Version;
        }
    }
}
