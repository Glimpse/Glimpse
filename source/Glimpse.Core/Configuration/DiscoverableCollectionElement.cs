using System.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node that configures the instance of <see cref="IDiscoverableCollection{T}"/> that Glimpse uses to automatically find and load types at runtime.
    /// </summary>
    /// <remarks>
    /// <c>DiscoverableCollectionElement</c> is used to configure many types, including: <see cref="IClientScript"/>, <see cref="IPipelineInspector"/>, <see cref="ISerializationConverter"/> and <see cref="ITab"/>. <see cref="IRuntimePolicy"/>'s use a specialized version of <c>DiscoverableCollectionElement</c> called <see cref="PolicyDiscoverableCollectionElement"/>.
    /// </remarks>
    public class DiscoverableCollectionElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets a value indicating whether Glimpse should automatically discover types at runtime.
        /// </summary>
        /// <value>
        /// <c>true</c> to automatically discover (default); otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("autoDiscover", DefaultValue = true)]
        public bool AutoDiscover
        {
            get { return (bool)base["autoDiscover"]; }
            set { base["autoDiscover"] = value; }
        }

        /// <summary>
        /// Gets or sets the file path to the automatic discovery location or a particular Glimpse type. This property overrides the globally configured <c>DiscoveryLocation</c> in <see cref="Section"/>.
        /// </summary>
        /// <value>
        /// The absolute or relative file path to the automatic discovery location. 
        /// Relative paths are rooted from <c>AppDomain.CurrentDomain.BaseDirectory</c>, or the equivalent shadow copy directory when appropriate.
        /// </value>
        [ConfigurationProperty("discoveryLocation", DefaultValue = Section.DefaultLocation)]
        public string DiscoveryLocation
        {
            get { return (string)base["discoveryLocation"]; }
            set { base["discoveryLocation"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of types for Glimpse to ignore when they are automatically discovered.
        /// </summary>
        [ConfigurationProperty("ignoredTypes")]
        public TypeElementCollection IgnoredTypes
        {
            get { return (TypeElementCollection)base["ignoredTypes"]; }
            set { base["ignoredTypes"] = value; }
        }
    }
}