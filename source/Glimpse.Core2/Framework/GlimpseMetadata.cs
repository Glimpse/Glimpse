using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata()
        {
            PluginMetadata = new Dictionary<string, PluginMetadata>();
        }

        public string Version{ get; set; }
        public IDictionary<string,PluginMetadata> PluginMetadata { get; set; }
    }

    public class PluginMetadata
    {
        public string DocumentationUri { get; set; }

        public bool HasMetadata { get { return !string.IsNullOrEmpty(DocumentationUri); }
        }
    }
}