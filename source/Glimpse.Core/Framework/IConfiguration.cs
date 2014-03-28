using System;
using System.Collections.Generic;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines properties to provide access to system providers, stores, collections,
    /// factories, etc.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets configured <see cref="ILogger"/>.
        /// </summary>
        /// <returns>The configured <see cref="ILogger"/> which defaults to a <see cref="NullLogger" /> in case the configured log level is set 
        /// to <see cref="LoggingLevel.Off"/> or with a <see cref="NLog.Logger" /> (leveraging the <see href="http://nlog-project.org/">NLog</see> project)
        /// for all other log levels. The configured logger can be replaced with a call to <see cref="ReplaceLogger"/>.
        /// </returns>
        ILogger Logger { get; }

        /// <summary>
        /// Replaces the current configured logger with the <paramref name="logger" /> specified.
        /// Keep in mind that the <paramref name="logger" /> will be ignored if the configured log level is set to <see cref="LoggingLevel.Off"/>
        /// </summary>
        /// <param name="logger">The logger to use from now on</param>
        /// <returns>This configuration</returns>
        IConfiguration ReplaceLogger(ILogger logger);

        /// <summary>
        /// Gets the client scripts.
        /// </summary>
        /// <value>The client scripts.</value>
        ICollection<IClientScript> ClientScripts { get; }

        /// <summary>
        /// Gets the HTML encoder.
        /// </summary>
        /// <value>The HTML encoder.</value>
        IHtmlEncoder HtmlEncoder { get; }

        IConfiguration ReplaceHtmlEncoder(IHtmlEncoder htmlEncoder);

        /// <summary>
        /// Gets the persistence store.
        /// </summary>
        /// <value>The persistence store.</value>
        IPersistenceStore PersistenceStore { get; }

        IConfiguration ReplacePersistenceStore(IPersistenceStore persistenceStore);

        /// <summary>
        /// Gets the inspectors.
        /// </summary>
        /// <value>The inspectors.</value>
        ICollection<IInspector> Inspectors { get; }

        /// <summary>
        /// Gets the resource endpoint.
        /// </summary>
        /// <value>The resource endpoint.</value>
        IResourceEndpointConfiguration ResourceEndpoint { get; }

#warning should this be allowed? Is this not the responsibility of the framework provider?
        IConfiguration ReplaceResourceEndpoint(IResourceEndpointConfiguration resourceEndpointConfiguration);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        ICollection<IResource> Resources { get; }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <value>The serializer.</value>
        ISerializer Serializer { get; }

        IConfiguration ReplaceSerializer(ISerializer serializer);

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        /// <value>The tabs.</value>
        ICollection<ITab> Tabs { get; }

        /// <summary>
        /// Gets the metadata extensions.
        /// </summary>
        /// <value>The metadata extensions.</value>
        ICollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets the tab metadata extensions.
        /// </summary>
        /// <value>The tab metadata extensions.</value>
        ICollection<ITabMetadata> TabMetadata { get; }

        /// <summary>
        /// Gets the tab instance metadata extensions.
        /// </summary>
        /// <value>The tab metadata extensions.</value>
        ICollection<IInstanceMetadata> InstanceMetadata { get; }

        [Obsolete]
        ICollection<IDisplay> Displays { get; }

        /// <summary>
        /// Gets the runtime policies.
        /// </summary>
        /// <value>The runtime policies.</value>
        ICollection<IRuntimePolicy> RuntimePolicies { get; }

        /// <summary>
        /// Gets the default resource.
        /// </summary>
        /// <value>The default resource.</value>
        IResource DefaultResource { get; }

        IConfiguration ReplaceDefaultResource(IResource defaultResource);

        /// <summary>
        /// Gets the default runtime policy.
        /// </summary>
        /// <value>The default runtime policy.</value>
        RuntimePolicy DefaultRuntimePolicy { get; }

        /// <summary>
        /// Gets the proxy factory.
        /// </summary>
        /// <value>The proxy factory.</value>
        IProxyFactory ProxyFactory { get; }

        IConfiguration ReplaceProxyFactory(IProxyFactory proxyFactory);

        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; }

        IConfiguration ReplaceMessageBroker(IMessageBroker messageBroker);

        /// <summary>
        /// Gets the endpoint base URI.
        /// </summary>
        /// <value>The endpoint base URI.</value>
        string EndpointBaseUri { get; }

        /// <summary>
        /// Gets the configuration hash.
        /// </summary>
        /// <value>The hash.</value>
        string Hash { get; }

        /// <summary>
        /// Gets the version of Glimpse core.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; }

        /// <summary>
        /// Gets the configured <see cref="ICurrentGlimpseRequestIdTracker"/>
        /// </summary>
        ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; }

        IConfiguration ReplaceCurrentGlimpseRequestIdTracker(ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker);
    }
}