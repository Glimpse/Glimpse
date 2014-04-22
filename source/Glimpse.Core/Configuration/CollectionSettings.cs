using System;
using System.Linq;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents the settings for a collection
    /// </summary>
    public class CollectionSettings
    {
        private CustomConfiguration[] CustomConfigurations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionSettings" /> class
        /// </summary>
        public CollectionSettings(
            Type[] typesToIgnore,
            CustomConfiguration[] customConfigurations,
            bool autoDiscover = false,
            string discoveryLocation = null)
        {
            Guard.ArgumentNotNull(typesToIgnore, "typesToIgnore");
            Guard.ArgumentNotNull(customConfigurations, "customConfigurations");

            TypesToIgnore = typesToIgnore;
            CustomConfigurations = customConfigurations;
            AutoDiscover = autoDiscover;
            DiscoveryLocation = discoveryLocation;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Glimpse should automatically discover types at runtime.
        /// </summary>
        /// <value>
        /// <c>true</c> to automatically discover (default); otherwise, <c>false</c>.
        /// </value>
        public bool AutoDiscover { get; private set; }

        /// <summary>
        /// Gets or sets the file path to the automatic discovery location or a particular Glimpse type. This property overrides the globally configured <c>DiscoveryLocation</c> in <see cref="Section"/>.
        /// </summary>
        /// <value>
        /// The absolute or relative file path to the automatic discovery location. 
        /// Relative paths are rooted from <c>AppDomain.CurrentDomain.BaseDirectory</c>, or the equivalent shadow copy directory when appropriate.
        /// </value>
        public string DiscoveryLocation { get; private set; }

        /// <summary>
        /// Gets the list of types for Glimpse to ignore if they should be discovered.
        /// </summary>
        public Type[] TypesToIgnore { get; private set; }

        /// <summary>
        /// Gets the custom configuration for the given configuration key
        /// </summary>
        /// <param name="configurationKey">The configuration key</param>
        /// <returns>The corresponding custom configuration or <code>null</code> if none has been found</returns>
        public string GetCustomConfiguration(string configurationKey)
        {
            return GetCustomConfiguration(configurationKey, null);
        }

        /// <summary>
        /// Gets the custom configuration for the given configuration key and type
        /// </summary>
        /// <param name="configurationKey">The configuration key</param>
        /// <param name="configurationFor">The type for which the configuration should apply</param>
        /// <returns>The corresponding custom configuration or <code>null</code> if none has been found</returns>
        public string GetCustomConfiguration(string configurationKey, Type configurationFor)
        {
            var customConfigurationsElementsForKey = CustomConfigurations
                .Where(customConfigurationElement => string.Equals(customConfigurationElement.Key, configurationKey, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            // return null if there is no configuration defined
            if (customConfigurationsElementsForKey.Count == 0)
            {
                return null;
            }

            // return the value, but if the configurationFor has a value then the type must be specified explicitly in the configuration
            if (customConfigurationsElementsForKey.Count == 1)
            {
                var customConfiguration = customConfigurationsElementsForKey[0];
                if (configurationFor != null && customConfiguration.Type != configurationFor)
                {
                    throw new GlimpseException(string.Format(
                        "Found custom configuration with name '{0}' but it was defined for type '{1}' instead of '{2}'",
                        configurationKey,
                        customConfiguration.Type != null ? customConfiguration.Type.FullName : "untyped",
                        configurationFor.FullName));
                }

                return customConfiguration.ConfigurationContent;
            }

            // there are multiple elements with the same key, which is not a problem as long as they all have a type defined and the one we need is
            // available as well
            if (customConfigurationsElementsForKey.Any(customConfigurationElement => customConfigurationElement.Type == null))
            {
                throw new GlimpseException(string.Format(
                    "Found {0} custom configurations for name '{1}' but not all of them have explicitly specified the type it is for.",
                    customConfigurationsElementsForKey.Count,
                    configurationKey));
            }

            // maybe they provided multiple elements for the same key and type which is bad as well, they should merge it
            var numberOfElementsForKeyAndType = customConfigurationsElementsForKey.Count(customConfigurationElement => customConfigurationElement.Type == configurationFor);
            if (numberOfElementsForKeyAndType != 1)
            {
                throw new GlimpseException(string.Format(
                    "Found {0} custom configurations for name '{1}' and type '{2}', please merge them.",
                    numberOfElementsForKeyAndType,
                    configurationKey,
                    configurationFor));
            }

            return customConfigurationsElementsForKey.Single(
                customConfigurationElement => customConfigurationElement.Type == configurationFor).ConfigurationContent;
        }
    }
}