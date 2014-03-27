using System;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The root Glimpse configuration node, that provides Glimpse configuration options in a site's <c>web.config</c>.
    /// </summary>
    /// <remarks>
    /// Before using, the <c>Section</c> class must be added to the <c>configSections</c> section of <c>web.config</c>, as shown in the examples. However, usually a NuGet package will make the necessary <c>web.config</c> changes automatically.
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <configuration>
    ///     <configSections>
    ///         <section name="glimpse" type="Glimpse.Core.Configuration.Section, Glimpse.Core" />
    ///         <!-- Additional section registrations -->
    ///     </configSections>
    ///     <!-- Additional configuration nodes -->
    /// </configuration>
    /// ]]>
    /// </code>
    /// </example>
    /// </remarks>
    public class Section : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the logging settings used by Glimpse.
        /// </summary>
        /// <remarks>
        /// Glimpse logging is mostly used to diagnose problems with Glimpse itself. Logging is off by default.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <logging level="Trace" />
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("logging")]
        public LoggingElement Logging
        {
            get { return (LoggingElement)base["logging"]; }
            set { base["logging"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IClientScript"/>s that are injected into a page response by Glimpse.
        /// </summary>
        /// <remarks>
        /// By default, client scripts are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <clientScripts autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </clientScripts>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("clientScripts")]
        public DiscoverableCollectionElement ClientScripts
        {
            get { return (DiscoverableCollectionElement)base["clientScripts"]; }
            set { base["clientScripts"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IInspector"/>s that used to gather information about a site during Http requests.
        /// </summary>
        /// <remarks>
        /// By default, inspectors are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <inspectors autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </inspectors>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("inspectors")]
        public DiscoverableCollectionElement Inspectors
        {
            get { return (DiscoverableCollectionElement)base["inspectors"]; }
            set { base["inspectors"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IResource"/>s that provide endpoints to access server side data.
        /// </summary>
        /// <remarks>
        /// By default, resources are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <resources autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </resources>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("resources")]
        public DiscoverableCollectionElement Resources
        {
            get { return (DiscoverableCollectionElement)base["resources"]; }
            set { base["resources"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ITab"/>s that Glimpse will render in the client.
        /// </summary>
        /// <remarks>
        /// By default, tabs are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <tabs autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </tabs>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("tabs")]
        public DiscoverableCollectionElement Tabs
        {
            get { return (DiscoverableCollectionElement)base["tabs"]; }
            set { base["tabs"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IMetadata"/>s that Glimpse will use to build the metadata.
        /// </summary>
        /// <remarks>
        /// By default, tabs are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <tabs autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </tabs>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("metadata")]
        public DiscoverableCollectionElement Metadata
        {
            get { return (DiscoverableCollectionElement)base["metadata"]; }
            set { base["metadata"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ITabMetadata"/>s that Glimpse will use in the tab metadata.
        /// </summary>
        /// <remarks>
        /// By default, tabs are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <tabMetadata autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </tabMetadata>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("tabMetadata")]
        public DiscoverableCollectionElement TabMetadata
        {
            get { return (DiscoverableCollectionElement)base["tabMetadata"]; }
            set { base["tabMetadata"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IInstanceMetadata"/>s that Glimpse will use in the instance metadata.
        /// </summary>
        /// <remarks>
        /// By default, instance metadata is discovered at runtime but that behavior, plus the location of where they 
        /// are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <instanceMetadata autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </instanceMetadata>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("instanceMetadata")]
        public DiscoverableCollectionElement InstanceMetadata
        {
            get { return (DiscoverableCollectionElement)base["instanceMetadata"]; }
            set { base["instanceMetadata"] = value; }
        }

        [ConfigurationProperty("displays")]
        public DiscoverableCollectionElement Displays
        {
            get { return (DiscoverableCollectionElement)base["displays"]; }
            set { base["displays"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="IRuntimePolicy"/>s that Glimpse will use to determine how it can manipulate any given Http response.
        /// </summary>
        /// <remarks>
        /// By default, runtime policies are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <runtimePolicies autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </runtimePolicies>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("runtimePolicies")]
        public DiscoverableCollectionElement RuntimePolicies
        {
            get { return (DiscoverableCollectionElement)base["runtimePolicies"]; }
            set { base["runtimePolicies"] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="ISerializationConverter"/>s that Glimpse will use alter the default serialization output of any given type.
        /// </summary>
        /// <remarks>
        /// By default, serialization converters are discovered at runtime but that behavior, plus the location of where they are found, and which ones should be ignored is configurable.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <serializationConverters autoDiscover="true" discoveryLocation="bin\debug">
        ///         <ignoredTypes>
        ///             <add type="{Namespace.Type, AssemblyName}"/>
        ///         </ignoredTypes>
        ///     </serializationConverters>
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("serializationConverters")]
        public DiscoverableCollectionElement SerializationConverters
        {
            get { return (DiscoverableCollectionElement)base["serializationConverters"]; }
            set { base["serializationConverters"] = value; }
        }

        /// <summary>
        /// Gets or sets the default runtime policy. Glimpse will never be allowed to do more with a Http response than is specified in <c>DefaultRuntimePolicy</c>.
        /// </summary>
        /// <remarks>
        /// By default, the default runtime policy of Glimpse is <c>Off</c>. Setting the base policy to <c>On</c> will allow Glimpse to fully function.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("defaultRuntimePolicy", DefaultValue = RuntimePolicy.Off)]
        public RuntimePolicy DefaultRuntimePolicy
        {
            get { return (RuntimePolicy)base["defaultRuntimePolicy"]; }
            set { base["defaultRuntimePolicy"] = value; }
        }

        /// <summary>
        /// Gets or sets an <see cref="IServiceLocator"/> Glimpse will use to override default type resolution.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd" serviceLocatorType="{Namespace.Type, AssemblyName}">
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("serviceLocatorType", DefaultValue = null)]
        [TypeConverter(typeof(TypeConverter))]
        public Type ServiceLocatorType
        {
            get { return (Type)base["serviceLocatorType"]; }
            set { base["serviceLocatorType"] = value; }
        }

        /// <summary>
        /// Gets or sets the base Uri endpoint for accessing all resources. The <c>EndpointBaseUri</c> is leveraged by an instance of <see cref="ResourceEndpointConfiguration"/> in order to generate dynamic resource Uris.
        /// </summary>
        /// <remarks>
        /// The <c>EndpointBaseUri</c> is required.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("endpointBaseUri", DefaultValue = null, IsRequired = true)]
        public string EndpointBaseUri
        {
            get { return (string)base["endpointBaseUri"]; }
            set { base["endpointBaseUri"] = value; }
        }

        /// <summary>
        /// Gets or sets the path Glimpse will use to auto discover all types.
        /// </summary>
        /// <remarks>
        /// The <c>DiscoveryLocation</c> defaults to <c>AppDomain.CurrentDomain.BaseDirectory</c>. Discovery locations for each type can be overwritten at the type level in <c>web.config</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd" discoveryLocation="bin\debug">
        ///     <!-- Additional Glimpse configuration nodes -->
        /// </glimpse>
        /// ]]>
        /// </code>
        /// </example>
        [ConfigurationProperty("discoveryLocation")]
        public string DiscoveryLocation
        {
            get { return (string)base["discoveryLocation"]; }
            set { base["discoveryLocation"] = value; }
        }
    }
}