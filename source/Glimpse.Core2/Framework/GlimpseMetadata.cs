using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata()
        {
            plugins = new Dictionary<string, PluginMetadata>();

            //TODO: this is really bad... Nik needs to work on how we want to do this. Version numbers need to be taken intoaccount too
            paths = new { history = "History", paging = "Pager", ajax = "Ajax", config = "Config", logo = "/Glimpse.axd?n=logo.png&Version=1.0.0", sprite = "/Glimpse.axd?n=sprite.png&Version=1.0.0", popup = "test-popup.html" };
        }

        public string version { get; set; }
        public IDictionary<string,PluginMetadata> plugins { get; set; }
        public object paths { get; set; }
    }

    public class PluginMetadata
    {
        public string DocumentationUri { get; set; }

        public bool HasMetadata { get { return !string.IsNullOrEmpty(DocumentationUri); }
        }
    }
}