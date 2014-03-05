using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public interface IReadonlyConfiguration
    {
        /// <summary>
        /// Gets the current requestId tracker.
        /// </summary>
        /// <value>The current requestId tracker.</value>
        ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; }

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

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        ILogger Logger { get; }

        /// <summary>
        /// Gets the persistence store.
        /// </summary>
        /// <value>The persistence store.</value>
        IPersistenceStore PersistenceStore { get; }

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

        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; }

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
    }
}