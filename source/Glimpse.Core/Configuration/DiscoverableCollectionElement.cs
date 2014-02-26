using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node that configures the instance of <see cref="IDiscoverableCollection{T}"/> that Glimpse uses to automatically find and load types at runtime.
    /// </summary>
    /// <remarks>
    /// <c>DiscoverableCollectionElement</c> is used to configure many types, including: <see cref="IClientScript"/>, <see cref="IInspector"/>, <see cref="ISerializationConverter"/>, <see cref="ITab"/> and <see cref="IRuntimePolicy"/>'s
    /// </remarks>
    public class DiscoverableCollectionElement : ConfigurationElement
    {
        private List<CustomConfigurationElement> CustomConfigurationElements { get; set; }

        public DiscoverableCollectionElement()
        {
            AutoDiscover = true;
            DiscoveryLocation = Section.DefaultLocation;
            IgnoredTypes = new Type[0];
            CustomConfigurationElements = new List<CustomConfigurationElement>();
        }

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reader.ReadOuterXml());

            var autoDiscoverAttribute = doc.DocumentElement.Attributes["autoDiscover"];
            if (autoDiscoverAttribute != null)
            {
                AutoDiscover = bool.Parse(autoDiscoverAttribute.Value);
            }

            var discoveryLocationAttribute = doc.DocumentElement.Attributes["discoveryLocation"];
            if (discoveryLocationAttribute != null)
            {
                DiscoveryLocation = discoveryLocationAttribute.Value;
            }

            foreach (XmlNode element in doc.DocumentElement.ChildNodes)
            {
                if (element.Name.ToLower() == "ignoredtypes")
                {
                    List<Type> ignoredTypes = new List<Type>();
                    foreach (XmlNode typeElement in element.ChildNodes)
                    {
                        if (typeElement.Name.ToLower() == "add")
                        {
                            var typeAttribute = typeElement.Attributes["type"];
                            if (typeAttribute == null)
                            {
                                throw new GlimpseException("type attribute missing on element that adds a type to ignore.");
                            }

                            ignoredTypes.Add((Type)new TypeConverter().ConvertFrom(null, null, typeAttribute.Value));
                        }
                        else
                        {
                            throw new GlimpseException("Only elements with name 'add' are allowed when adding types to ignore.");
                        }
                    }

                    IgnoredTypes = ignoredTypes.ToArray();
                }
                else
                {
                    var configurationElement = new CustomConfigurationElement();
                    configurationElement.Key = element.Name;

                    XmlAttribute typeAttribute = element.Attributes["type"];
                    if (typeAttribute != null)
                    {
                        configurationElement.Type = (Type)new TypeConverter().ConvertFrom(null, null, typeAttribute.Value);
                    }

                    configurationElement.ConfigurationContent = element.OuterXml;
                    CustomConfigurationElements.Add(configurationElement);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Glimpse should automatically discover types at runtime.
        /// </summary>
        /// <value>
        /// <c>true</c> to automatically discover (default); otherwise, <c>false</c>.
        /// </value>
        public bool AutoDiscover { get; set; }

        /// <summary>
        /// Gets or sets the file path to the automatic discovery location or a particular Glimpse type. This property overrides the globally configured <c>DiscoveryLocation</c> in <see cref="Section"/>.
        /// </summary>
        /// <value>
        /// The absolute or relative file path to the automatic discovery location. 
        /// Relative paths are rooted from <c>AppDomain.CurrentDomain.BaseDirectory</c>, or the equivalent shadow copy directory when appropriate.
        /// </value>
        public string DiscoveryLocation { get; set; }

        /// <summary>
        /// Gets the list of types for Glimpse to ignore when they are automatically discovered.
        /// </summary>
        public Type[] IgnoredTypes { get; set; }

        public string GetCustomConfiguration(string configurationKey)
        {
            return GetCustomConfiguration(configurationKey, null);
        }

        public string GetCustomConfiguration(string configurationKey, Type configurationFor)
        {
            var customConfigurationsElementsForKey = CustomConfigurationElements
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