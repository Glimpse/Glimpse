using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Framework
{
    public static class Configuration
    {
        public static ConfigurationModel Current { get; set; }
    }

    public class ConfigurationModel
    {
        public string Currency { get; set; }
        
        public double TaxRate { get; set; }
        
        public int DefaultCategory { get; set; }

        public double MarkupRate { get; set; }
    }
}