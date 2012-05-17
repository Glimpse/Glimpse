using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata()
        {
            Plugins = new Dictionary<string, PluginMetadata>();

            //TODO: this is really bad... Nik needs to work on how we want to do this. Version numbers need to be taken into account too
            Resources = new Dictionary<string, string>//TODO: once the resources below are implemented, this constructor should just instantiate paths.
                        {
                            {"history", "NEED TO IMPLMENT RESOURCE History"},//TODO: Implement resource
                            {"paging", "NEED TO IMPLMENT RESOURCE Pager"},//TODO: Implement resource
                            {"ajax", "NEED TO IMPLMENT RESOURCE Ajax"},//TODO: Implement resource
                            {"popup", "NEED TO IMPLMENT RESOURCE test-popup.html"},//TODO: Implement resource
                        };
        }

        public string Version { get; set; }
        public IDictionary<string,PluginMetadata> Plugins { get; set; }
        public IDictionary<string,string> Resources { get; set; }
    }

    public class PluginMetadata
    {
        public string DocumentationUri { get; set; }
        public bool HasMetadata { get { return !string.IsNullOrEmpty(DocumentationUri); } }
    }
}