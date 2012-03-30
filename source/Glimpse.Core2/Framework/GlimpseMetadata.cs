using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata()
        {
            plugins = new Dictionary<string, PluginMetadata>();
        }

        public string version{ get; set; }
        public IDictionary<string,PluginMetadata> plugins { get; set; }
    }

    public class PluginMetadata
    {
        public string DocumentationUri { get; set; }

        public bool HasMetadata { get { return !string.IsNullOrEmpty(DocumentationUri); }
        }
    }
}