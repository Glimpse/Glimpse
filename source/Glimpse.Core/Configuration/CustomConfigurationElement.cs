using System;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents a custom configuration element
    /// </summary>
    public class CustomConfigurationElement
    {
        /// <summary>
        /// The custom configuration key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The <see cref="Type"/> for which the custom configuration applies
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The actual custom configuration content
        /// </summary>
        public string ConfigurationContent { get; set; }
    }
}
