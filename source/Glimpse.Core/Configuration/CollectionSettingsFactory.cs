using System;
using System.Collections.Generic;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Factory of <see cref="CollectionSettings" /> instances
    /// </summary>
    public class CollectionSettingsFactory
    {
        private string DefaultDiscoveryLocation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionSettingsFactory" /> class
        /// </summary>
        /// <param name="defaultDiscoveryLocation">The default discovery location to use if the configuration has none specified.</param>
        public CollectionSettingsFactory(string defaultDiscoveryLocation)
        {
            DefaultDiscoveryLocation = defaultDiscoveryLocation;
        }

        /// <summary>
        /// Creates a <see cref="CollectionSettings" /> based on the given <paramref name="xmlConfiguration" />
        /// </summary>
        /// <param name="xmlConfiguration">The xml configuration.</param>
        /// <returns>The <see cref="CollectionSettings" /> based on the given <paramref name="xmlConfiguration"/></returns>
        public CollectionSettings Create(XmlElement xmlConfiguration)
        {
            if (xmlConfiguration == null)
            {
                return new CollectionSettings(new Type[0], new CustomConfiguration[0]);
            }

            bool autoDiscover = true;
            string discoveryLocation = null;
            List<Type> typesToIgnore = new List<Type>();
            List<CustomConfiguration> customConfigurations = new List<CustomConfiguration>();

            var autoDiscoverAttribute = xmlConfiguration.Attributes["autoDiscover"];
            if (autoDiscoverAttribute != null)
            {
                autoDiscover = bool.Parse(autoDiscoverAttribute.Value);
            }

            var discoveryLocationAttribute = xmlConfiguration.Attributes["discoveryLocation"];
            if (discoveryLocationAttribute != null)
            {
                discoveryLocation = discoveryLocationAttribute.Value;
            }

            if (string.IsNullOrEmpty(discoveryLocation))
            {
                // no discoverylocation has been specified for this specific discoverable collection, so we assign the default discoverylocation
                discoveryLocation = DefaultDiscoveryLocation;
            }

            foreach (XmlNode element in xmlConfiguration.ChildNodes)
            {
                if (element == null)
                {
                    continue;
                }

                if (element.Name.ToLower() == "ignoredtypes")
                {
                    foreach (XmlNode typeElement in element.ChildNodes)
                    {
                        if (typeElement.Name.ToLower() == "add")
                        {
                            typesToIgnore.Add(
                                typeElement.GetTypeFromTypeAttribute(true, "type attribute missing on element that adds a type to ignore."));
                        }
                        else
                        {
                            throw new GlimpseException("Only elements with name 'add' are allowed when adding types to ignore.");
                        }
                    }
                }
                else
                {
                    string key = element.Name;
                    string configurationContent = element.OuterXml;
                    customConfigurations.Add(new CustomConfiguration(key, configurationContent, element.GetTypeFromTypeAttribute(false)));
                }
            }

            return new CollectionSettings(
                typesToIgnore.ToArray(),
                customConfigurations.ToArray(),
                autoDiscover,
                discoveryLocation);
        }
    }
}