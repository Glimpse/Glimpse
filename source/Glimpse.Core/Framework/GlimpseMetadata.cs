using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata()
        {
            Plugins = new Dictionary<string, PluginMetadata>();

            Resources = new Dictionary<string, string>();
        }

        public string Version { get; set; }
        
        public IDictionary<string, PluginMetadata> Plugins { get; set; }
        
        public IDictionary<string, string> Resources { get; set; }
    }
}