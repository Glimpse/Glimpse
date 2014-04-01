using System;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents a custom configuration
    /// </summary>
    public class CustomConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionSettings" /> class
        /// </summary>
        /// <param name="key">The custom configuration key</param>
        /// <param name="configurationContent">The actual custom configuration content</param>
        /// <param name="type">The optional <see cref="Type"/> for which the custom configuration explicitly applies</param>
        public CustomConfiguration(string key, string configurationContent, Type type = null)
        {
            Key = key;
            ConfigurationContent = configurationContent;
            Type = type;
        }

        /// <summary>
        /// Gets the custom configuration key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the optional <see cref="Type"/> for which the custom configuration explicitly applies
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the actual custom configuration content
        /// </summary>
        public string ConfigurationContent { get; private set; }
    }
}
