using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents the settings for a collection
    /// </summary>
    public class CollectionSettings
    {
        private Dictionary<string, CustomConfiguration[]> CustomConfigurationsByKey { get; set; }

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

            ValidateAndInitializeCustomConfigurations(customConfigurations);
            TypesToIgnore = typesToIgnore;
            AutoDiscover = autoDiscover;
            DiscoveryLocation = discoveryLocation;
        }

        private void ValidateAndInitializeCustomConfigurations(CustomConfiguration[] customConfigurations)
        {
            var customConfigurationsByKey = new Dictionary<string, CustomConfiguration[]>();

            var groupedCustomConfigurationsByKey = customConfigurations.GroupBy(customConfiguration => customConfiguration.Key);
            foreach (var groupedCustomConfigurationByKey in groupedCustomConfigurationsByKey)
            {
                var numberOfCustomConfigurationsForKey = groupedCustomConfigurationByKey.Count();
                if (numberOfCustomConfigurationsForKey != 1)
                {
                    // if multiple custom configurations have been specified with the same key, then all of them must have specified
                    // a unique corresponding configurator type for that key.
                    if (groupedCustomConfigurationByKey.Any(customConfiguration => customConfiguration.ConfiguratorType == null))
                    {
                        throw new GlimpseException(string.Format(
                            "Found {0} custom configurations for key '{1}' but not all of them defined their corresponding configurator type.",
                            numberOfCustomConfigurationsForKey,
                            groupedCustomConfigurationByKey.Key));
                    }
                    else
                    {
                        foreach (var groupedCustomConfigurationByKeyAndType in groupedCustomConfigurationByKey.GroupBy(customConfiguration => customConfiguration.ConfiguratorType))
                        {
                            var numberOfCustomerConfigurationsByKeyAndType = groupedCustomConfigurationByKeyAndType.Count();
                            if (numberOfCustomerConfigurationsByKeyAndType != 1)
                            {
                                throw new GlimpseException(string.Format(
                                    "Found {0} custom configurations for key '{1}' and type '{2}', please merge them.",
                                    numberOfCustomerConfigurationsByKeyAndType,
                                    groupedCustomConfigurationByKey.Key,
                                    groupedCustomConfigurationByKeyAndType.Key));
                            }
                        }
                    }
                }

                customConfigurationsByKey.Add(groupedCustomConfigurationByKey.Key, groupedCustomConfigurationByKey.ToArray());
            }

            // we will also check whether the same configurator type has been assigned to multiple custom configurations, as that wouldn't make sense either
            var groupedCustomConfigurationsByType = customConfigurations
                .Where(customConfiguration => customConfiguration.ConfiguratorType != null)
                .GroupBy(customConfiguration => customConfiguration.ConfiguratorType);

            foreach (var groupedCustomConfigurationByType in groupedCustomConfigurationsByType)
            {
                var numberOfCustomConfigurationsForType = groupedCustomConfigurationByType.Count();

                if (numberOfCustomConfigurationsForType != 1)
                {
                    throw new GlimpseException(string.Format(
                        "Found {0} custom configurations for type '{1}' which is not allowed. The following keys were involved : '{2}'.",
                        numberOfCustomConfigurationsForType,
                        groupedCustomConfigurationByType.Key,
                        string.Join(", ", groupedCustomConfigurationByType.Select(customConfiguration => customConfiguration.Key))));
                }
            }

            CustomConfigurationsByKey = customConfigurationsByKey;
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
        /// Gets the custom configuration for the given configuration key and type
        /// </summary>
        /// <param name="configurationKey">The configuration key</param>
        /// <param name="requestingConfigurator">The requesting configurator</param>
        /// <param name="defaultCustomConfigurationAllowed">
        /// Indicates whether or not the default custom configuration, if any, is allowed to be returned
        /// when no exact key/configuratorType match was found.</param>
        /// <returns>The corresponding custom configuration or <code>null</code> if none has been found</returns>
        public string GetCustomConfiguration(string configurationKey, Type requestingConfigurator, bool defaultCustomConfigurationAllowed)
        {
            CustomConfiguration[] customConfigurationsForKey;
            if (CustomConfigurationsByKey.TryGetValue(configurationKey, out customConfigurationsForKey))
            {
                var customConfigurationForKeyAndType = customConfigurationsForKey.SingleOrDefault(customConfiguration => customConfiguration.ConfiguratorType == requestingConfigurator);
                if (customConfigurationForKeyAndType != null)
                {
                    return customConfigurationForKeyAndType.ConfigurationContent;
                }
                else if (defaultCustomConfigurationAllowed)
                {
                    var defaultCustomConfiguration = customConfigurationsForKey.SingleOrDefault(customConfiguration => customConfiguration.ConfiguratorType == null);
                    return defaultCustomConfiguration != null ? defaultCustomConfiguration.ConfigurationContent : null;
                }
                else
                {
                    throw new GlimpseException(string.Format(
                        "Found {0} custom configurations for key '{1}' but they were defined for other configurator types and a default (if available) was not allowed either.",
                        customConfigurationsForKey.Length,
                        configurationKey));
                }
            }

            return null;
        }
    }
}