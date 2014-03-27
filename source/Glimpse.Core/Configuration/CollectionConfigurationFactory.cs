using System;
using System.Collections.Generic;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    public class CollectionConfigurationFactory
    {
        private XmlElement XmlConfiguration { get; set; }

        private string DefaultDiscoveryLocation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionConfigurationFactory" /> class
        /// </summary>
        /// <param name="xmlConfiguration">The xml configuration.</param>
        /// <param name="defaultDiscoveryLocation">The default discovery location to use if the configuration has none specified.</param>
        public CollectionConfigurationFactory(XmlElement xmlConfiguration, string defaultDiscoveryLocation)
        {
            Guard.ArgumentNotNull("xmlConfiguration", xmlConfiguration);
            XmlConfiguration = xmlConfiguration;
            DefaultDiscoveryLocation = defaultDiscoveryLocation;
        }

        public CollectionConfiguration Create()
        {
            bool autoDiscover = true;
            string discoveryLocation = null;
            List<Type> typesToIgnore = new List<Type>();
            List<CustomConfiguration> customConfigurations = new List<CustomConfiguration>();

            var autoDiscoverAttribute = XmlConfiguration.Attributes["autoDiscover"];
            if (autoDiscoverAttribute != null)
            {
                autoDiscover = bool.Parse(autoDiscoverAttribute.Value);
            }

            var discoveryLocationAttribute = XmlConfiguration.Attributes["discoveryLocation"];
            if (discoveryLocationAttribute != null)
            {
                discoveryLocation = discoveryLocationAttribute.Value;
            }

            if (string.IsNullOrEmpty(discoveryLocation))
            {
                // no discoverylocation has been specified for this specific discoverable collection, so we assign the default discoverylocation
                discoveryLocation = DefaultDiscoveryLocation;
            }

            foreach (XmlNode element in XmlConfiguration.ChildNodes)
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
                            var typeAttribute = typeElement.Attributes != null ? typeElement.Attributes["type"] : null;
                            if (typeAttribute == null)
                            {
                                throw new GlimpseException("type attribute missing on element that adds a type to ignore.");
                            }

                            typesToIgnore.Add(Type.GetType(typeAttribute.Value, true, true));
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
                    Type explicitType = null;

                    XmlAttribute typeAttribute = element.Attributes != null ? element.Attributes["type"] : null;
                    if (typeAttribute != null)
                    {
                        explicitType = Type.GetType(typeAttribute.Value, true, true);
                    }

                    customConfigurations.Add(new CustomConfiguration(key, configurationContent, explicitType));
                }
            }

            return new CollectionConfiguration(
                typesToIgnore.ToArray(),
                customConfigurations.ToArray(),
                autoDiscover,
                discoveryLocation);
        }
    }
}