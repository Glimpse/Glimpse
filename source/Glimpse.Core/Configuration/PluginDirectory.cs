using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Directory containing plugins
    /// </summary>
    public class PluginDirectory : ConfigurationElement
    {
        [ConfigurationProperty("directory")]
        public string Directory
        {
            get
            { 
                return this["directory"].ToString();
            }
            set { this["directory"] = value; }
        }

        public override string ToString()
        {
            return Directory;
        }
    }
}
