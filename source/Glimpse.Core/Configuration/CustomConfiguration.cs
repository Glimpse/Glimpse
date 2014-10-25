using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents a custom configuration
    /// </summary>
    public class CustomConfiguration
    {
        private static readonly Type configuratorInterfaceType = typeof(IConfigurator);

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionSettings" /> class
        /// </summary>
        /// <param name="key">The custom configuration key</param>
        /// <param name="configurationContent">The actual custom configuration content</param>
        /// <param name="configuratorType">The optional <see cref="Type"/> of an <see cref="IConfigurator"/> for which the custom configuration applies</param>
        public CustomConfiguration(string key, string configurationContent, Type configuratorType = null)
        {
            Guard.StringNotNullOrEmpty(key, "key");

            Key = key;
            ConfigurationContent = configurationContent;

            if (configuratorType != null && !configuratorInterfaceType.IsAssignableFrom(configuratorType))
            {
                throw new GlimpseException(string.Format(
                    "The configurator type '{0}' specified for the custom configuration with key '{1}' must implement '{2}'.",
                    configuratorType,
                    key,
                    configuratorInterfaceType));
            }

            ConfiguratorType = configuratorType;
        }

        /// <summary>
        /// Gets the custom configuration key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the optional <see cref="Type"/> of a <see cref="IConfigurator"/> for which the custom configuration applies
        /// </summary>
        public Type ConfiguratorType { get; private set; }

        /// <summary>
        /// Gets the actual custom configuration content
        /// </summary>
        public string ConfigurationContent { get; private set; }
    }
}